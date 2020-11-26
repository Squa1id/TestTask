using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.EntityFrameworkCore;

using OfficeOpenXml;

using TestTask.Data.Entities;
using TestTask.Data.Enums;
using TestTask.Data.ViewModels;
using TestTask.Data.ViewModels.EntityViewModels;

namespace TestTask.Data.Repositories {
	public class SmallBusinessSubjectAuditRepository : BaseRepository<SmallBusinessSubjectAudit, SmallBusinessSubjectAuditViewModel> {
		public SmallBusinessSubjectAuditRepository(Context context) : base(context) { }

		public DataTableResponseModel<SmallBusinessSubjectAuditViewModel> GetSmallBusinessSubjectAuditDataTable(int start, int length, int draw, string name, DateTime? startDate, DateTime? endDate) {
			try {
				var model = new DataTableResponseModel<SmallBusinessSubjectAuditViewModel>();
				var smallBusinessSubjectAudits = GetEntities<SmallBusinessSubjectAudit>(true,
						x => x.SmallBusinessSubject,
						x => x.SupervisoryAuthority);
				if (!string.IsNullOrEmpty(name)) {
					name = name.ToLower();
					smallBusinessSubjectAudits = smallBusinessSubjectAudits.Where(x => x.SmallBusinessSubject.Name.ToLower().Contains(name));
				}
				if (startDate.HasValue) {
					smallBusinessSubjectAudits = smallBusinessSubjectAudits.Where(x => x.BeginAuditDate >= startDate.Value);
				}
				if (endDate.HasValue) {
					smallBusinessSubjectAudits = smallBusinessSubjectAudits.Where(x => x.BeginAuditDate <= endDate.Value);
				}
				model.recordsFiltered = smallBusinessSubjectAudits.Count();
				var data = smallBusinessSubjectAudits.OrderBy(x => 0)
					.Skip(start).Take(length).Select(ModelToViewModel()).ToArray();
				model.data = data;
				model.draw = draw;
				return model;
			} catch (Exception e) {
				return DataTableResponseModel<SmallBusinessSubjectAuditViewModel>.ErrorResponse(e);
			}
		}

		public byte[] ExportSmallBusinessSubjectAudit(ExportType type, string name, DateTime startDate, DateTime endDate) {
			name = name?.ToLower();
			var smallBusinessSubjectAudits = GetEntities<SmallBusinessSubjectAudit>(true,
					x => x.SmallBusinessSubject,
					x => x.SupervisoryAuthority)
				.Where(x => string.IsNullOrEmpty(name) || x.SmallBusinessSubject.Name.ToLower().Contains(name)).Select(ModelToViewModel()).ToArray();
			using (var package = new ExcelPackage()) {
				var worksheet = package.Workbook.Worksheets.Add("Реестр");
				worksheet.Cells[1, 1].Value = "Проверяемый СМП";
				worksheet.Column(1).Width = 30;
				worksheet.Cells[1, 2].Value = "Контролирующий орган";
				worksheet.Column(2).Width = 30;
				worksheet.Cells[1, 3].Value = "Плановый период проверки";
				worksheet.Column(3).Width = 30;
				worksheet.Cells[1, 4].Value = "Плановая длительность";
				worksheet.Column(4).Width = 30;
				for (int i = 0; i < smallBusinessSubjectAudits.Length; i++) {
					worksheet.Cells[i + 2, 1].Value = smallBusinessSubjectAudits[i].SmallBusinessSubjectName;
					worksheet.Cells[i + 2, 2].Value = smallBusinessSubjectAudits[i].SupervisoryAuthorityName;
					worksheet.Cells[i + 2, 3].Value = $"{smallBusinessSubjectAudits[i].BeginAuditDate} - {smallBusinessSubjectAudits[i].EndAuditDate}";
					worksheet.Cells[i + 2, 4].Value = smallBusinessSubjectAudits[i].AuditDuration;
				}
				return package.GetAsByteArray();
			}
		}

		public void ImportSmallBusinessSubjectAudit(MemoryStream stream) {
			List<SmallBusinessSubjectAudit> newSmallBusinessSubjectAudits = new List<SmallBusinessSubjectAudit>();
			var smallBusinessSubjects = _context.SmallBusinessSubjects.AsNoTracking()
				.ToDictionary(x => x.PrimaryStateRegistrationNumber, x => x.Id);
			var supervisoryAuthorities = _context.SupervisoryAuthorities.AsNoTracking()
				.ToDictionary(x => x.Name.ToLower(), x => x.Id);
			using (var package = new ExcelPackage(stream)) {
				var worksheet = package.Workbook.Worksheets[0];
				for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++) {
					Guid smallBusinessSubjectId,
						supervisoryAuthorityId;
					DateTime beginAuditDate,
						endAuditDate;
					if (worksheet.Cells[i, 1].Value == null ||
						worksheet.Cells[i, 2].Value == null ||
						worksheet.Cells[i, 3].Value == null ||
						worksheet.Cells[i, 4].Value == null ||
						worksheet.Cells[i, 5].Value == null) {
						continue;
					}

					var stringDates = worksheet.Cells[i, 4].Value.ToString().Split('-');
					if (stringDates.Length != 2) {
						continue;
					}
					if (!smallBusinessSubjects.TryGetValue(worksheet.Cells[i, 2].Value.ToString(), out smallBusinessSubjectId) ||
						!supervisoryAuthorities.TryGetValue(worksheet.Cells[i, 3].Value.ToString().ToLower(), out supervisoryAuthorityId) ||
						!DateTime.TryParse(stringDates[0], out beginAuditDate) ||
						!DateTime.TryParse(stringDates[1], out endAuditDate)) {
						continue;
					}
					var auditDuration = (ushort)(double)worksheet.Cells[i, 5].Value;
					newSmallBusinessSubjectAudits.Add(new SmallBusinessSubjectAudit() {
						Id = Guid.NewGuid(),
						SmallBusinessSubjectId = smallBusinessSubjectId,
						SupervisoryAuthorityId = supervisoryAuthorityId,
						BeginAuditDate = beginAuditDate,
						EndAuditDate = endAuditDate,
						AuditDuration = auditDuration
					});
				}
				package.Stream.Close();
			}

			_context.SmallBusinessSubjectAudits.AddRange(newSmallBusinessSubjectAudits);
			_context.SaveChanges();
		}

		public void DeleteSmallBusinessSubjectAudits(Guid[] ids) {
			var smallBusinessSubjectAudits = GetEntities<SmallBusinessSubjectAudit>()
				.Where(x => ids.Contains(x.Id));
			_context.SmallBusinessSubjectAudits.RemoveRange(smallBusinessSubjectAudits);
			_context.SaveChanges();
		}

		public PairResponseModel<Guid>[] GetSmallBusinessSubjects() {
			try {
				return _context.SmallBusinessSubjects.AsNoTracking().Select(x => new PairResponseModel<Guid>(x.Id, x.Name)).ToArray();
			} catch {
				return new PairResponseModel<Guid>[0];
			}
		}

		public PairResponseModel<Guid>[] GetSupervisoryAuthorities() {
			try {
				return _context.SupervisoryAuthorities.AsNoTracking().Select(x => new PairResponseModel<Guid>(x.Id, x.Name)).ToArray();
			} catch {
				return new PairResponseModel<Guid>[0];
			}
		}

		public SmallBusinessSubjectAuditViewModel GetSmallBusinessSubjectAudit(Guid id) {
			try {
				return GetEntityViewModelById<SmallBusinessSubjectAudit>(id, true,
					x => x.SmallBusinessSubject,
					x => x.SupervisoryAuthority);
			} catch {
				return new SmallBusinessSubjectAuditViewModel();
			}
		}

		public void SaveSmallBusinessSubjectAudit(SmallBusinessSubjectAuditViewModel model) {
			SmallBusinessSubjectAudit entity;
			if (model.Id.HasValue) {
				entity = GetEntityById<SmallBusinessSubjectAudit>(model.Id.Value);
				entity.SmallBusinessSubjectId = model.SmallBusinessSubjectId;
				entity.SupervisoryAuthorityId = model.SupervisoryAuthorityId;
				entity.BeginAuditDate = DateTime.Parse(model.BeginAuditDate);
				entity.EndAuditDate = DateTime.Parse(model.EndAuditDate);
				entity.AuditDuration = model.AuditDuration;
			} else {
				entity = new SmallBusinessSubjectAudit(model.SmallBusinessSubjectId, model.SupervisoryAuthorityId,
					DateTime.Parse(model.BeginAuditDate), DateTime.Parse(model.EndAuditDate), model.AuditDuration);
				_context.SmallBusinessSubjectAudits.Add(entity);
			}
			_context.SaveChanges();
		}

		public override Func<SmallBusinessSubjectAudit, SmallBusinessSubjectAuditViewModel> ModelToViewModel() {
			return model => new SmallBusinessSubjectAuditViewModel {
				Id = model.Id,
				SmallBusinessSubjectId = model.SmallBusinessSubjectId,
				SupervisoryAuthorityId = model.SupervisoryAuthorityId,
				BeginAuditDate = model.BeginAuditDate.ToShortDateString(),
				EndAuditDate = model.EndAuditDate.ToShortDateString(),
				AuditDuration = model.AuditDuration,
				SmallBusinessSubjectName = model.SmallBusinessSubject.Name,
				SupervisoryAuthorityName = model.SupervisoryAuthority.Name
			};
		}
	}
}
