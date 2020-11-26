using System;
using System.Collections.Generic;
using System.Text;
using TestTask.Data.Entities;
using TestTask.Data.Interfaces;

namespace TestTask.Data.Entities {
	/// <summary>
	/// Проверка СМП
	/// </summary>
	public class SmallBusinessSubjectAudit: IKeyEntity {
		/// <summary>
		/// Идентификатор проверки
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// Идентификатор проверяемого СМП
		/// </summary>
		public Guid SmallBusinessSubjectId { get; set; }
		/// <summary>
		/// Проверяемый СМП
		/// </summary>
		public SmallBusinessSubject SmallBusinessSubject { get; set; }
		/// <summary>
		/// Идентификатор проверяющего органа
		/// </summary>
		public Guid SupervisoryAuthorityId { get; set; }
		/// <summary>
		/// Проверяемый СМП
		/// </summary>
		public SupervisoryAuthority SupervisoryAuthority { get; set; }
		/// <summary>
		/// Дата начала проверки
		/// </summary>
		public DateTime BeginAuditDate { get; set; }
		/// <summary>
		/// Дата окончания проверки
		/// </summary>
		public DateTime EndAuditDate { get; set; }
		/// <summary>
		/// Продолжительность проверки
		/// </summary>
		public ushort AuditDuration { get; set; }

		public SmallBusinessSubjectAudit(Guid smallBusinessSubjectId, Guid supervisoryAuthorityId, DateTime beginAuditDate, DateTime endAuditDate, ushort auditDuration) {
			this.Id = Guid.NewGuid();
			this.SmallBusinessSubjectId = smallBusinessSubjectId;
			this.SupervisoryAuthorityId = supervisoryAuthorityId;
			this.BeginAuditDate = beginAuditDate;
			this.EndAuditDate = endAuditDate;
			this.AuditDuration = auditDuration;
		}

		public SmallBusinessSubjectAudit() { }
	}
}
