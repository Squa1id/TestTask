using System;
using System.Collections.Generic;
using System.Text;
using TestTask.Data.Entities;

namespace TestTask.Data.ViewModels.EntityViewModels {
	public class SmallBusinessSubjectAuditViewModel {
		/// <summary>
		/// Идентификатор проверки
		/// </summary>
		public Guid? Id { get; set; }
		/// <summary>
		/// Идентификатор проверяемого СМП
		/// </summary>
		public Guid SmallBusinessSubjectId { get; set; }
		/// <summary>
		/// Проверяемый СМП
		/// </summary>
		public string SmallBusinessSubjectName { get; set; }
		/// <summary>
		/// Идентификатор проверяющего органа
		/// </summary>
		public Guid SupervisoryAuthorityId { get; set; }
		/// <summary>
		/// Проверяемый СМП
		/// </summary>
		public string SupervisoryAuthorityName { get; set; }
		/// <summary>
		/// Дата начала проверки
		/// </summary>
		public string BeginAuditDate { get; set; }
		/// <summary>
		/// Дата окончания проверки
		/// </summary>
		public string EndAuditDate { get; set; }
		/// <summary>
		/// Продолжительность проверки
		/// </summary>
		public ushort AuditDuration { get; set; }
	}
}
