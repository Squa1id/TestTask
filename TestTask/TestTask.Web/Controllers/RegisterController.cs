using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TestTask.Data;
using TestTask.Data.Entities;
using TestTask.Data.Enums;
using TestTask.Data.Repositories;
using TestTask.Data.ViewModels.EntityViewModels;
using TestTask.Web.Models;

namespace TestTask.Web.Controllers {
	public class RegisterController : Controller {
		private readonly SmallBusinessSubjectAuditRepository _smallBusinessSubjectAuditRepository;

		public RegisterController(Context context) {
			this._smallBusinessSubjectAuditRepository = new SmallBusinessSubjectAuditRepository(context);
		}

		public IActionResult Index() {
			return View();
		}

		[HttpPost]
		public IActionResult GetSmallBusinessSubjectAuditDataTable(int start, int length, int draw, string name, DateTime? startDate, DateTime? endDate) {
			var data = _smallBusinessSubjectAuditRepository.GetSmallBusinessSubjectAuditDataTable(start, length, draw, name, startDate, endDate);
			return Json(data);
		}

		[HttpPost]
		public void DeleteSmallBusinessSubjectAudits(Guid[] ids) {
			_smallBusinessSubjectAuditRepository.DeleteSmallBusinessSubjectAudits(ids);
		}

		[HttpPost]
		public void ImportSmallBusinessSubjectAudit(IFormFile file) {
			var stream = new MemoryStream();
			file.CopyTo(stream);
			_smallBusinessSubjectAuditRepository.ImportSmallBusinessSubjectAudit(stream);
		}

		[HttpPost]
		public IActionResult GetSmallBusinessSubjects() {
			var data = _smallBusinessSubjectAuditRepository.GetSmallBusinessSubjects();
			return Json(data);
		}

		[HttpPost]
		public IActionResult GetSupervisoryAuthorities() {
			var data = _smallBusinessSubjectAuditRepository.GetSupervisoryAuthorities();
			return Json(data);
		}

		[HttpPost]
		public IActionResult GetSmallBusinessSubjectAudit(Guid id) {
			var data = _smallBusinessSubjectAuditRepository.GetSmallBusinessSubjectAudit(id);
			return Json(data);
		}

		[HttpPost]
		public void SaveSmallBusinessSubjectAudit(SmallBusinessSubjectAuditViewModel model) {
			_smallBusinessSubjectAuditRepository.SaveSmallBusinessSubjectAudit(model);
		}

		[HttpGet]
		public IActionResult ExportSmallBusinessSubjectAudit(ExportType type, string name, string startDate, string endDate) {
			DateTime.TryParse(startDate, out DateTime startDateTime);
			DateTime.TryParse(endDate, out DateTime endDateTime);
			var data = _smallBusinessSubjectAuditRepository.ExportSmallBusinessSubjectAudit(type, name, startDateTime, endDateTime);
			var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			var fileName = "Register.xlsx";
			return File(data, contentType, fileName);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
