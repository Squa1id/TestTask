using System;
using System.Collections.Generic;

using TestTask.Data.Entities;
using TestTask.Data.Interfaces;

namespace TestTask.Data.Entities {
	/// <summary>
	/// СМП
	/// </summary>
	public class SmallBusinessSubject : IKeyEntity, INameEntity {
		public Guid Id { get; set; }
		/// <summary>
		/// Название организации
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// ОГРН
		/// </summary>
		public string PrimaryStateRegistrationNumber { get; set; }
		/// <summary>
		/// ИНН
		/// </summary>
		public string TaxpayerIdentificationNumber { get; set; }
		/// <summary>
		/// Уставный капитал
		/// </summary>
		public decimal AuthorizedCapital { get; set; }
		/// <summary>
		/// Руководитель
		/// </summary>
		public string DirectorName { get; set; }
		/// <summary>
		/// Дата регистрации
		/// </summary>
		public DateTime RegistrationDate { get; set; }

		public ICollection<SmallBusinessSubjectAudit> SmallBusinessSubjectAudits { get; set; }

		public SmallBusinessSubject(string name, string primaryStateRegistrationNumber, string taxpayerIdentificationNumber, decimal authorizedCapital, string directorName, DateTime registrationDate) {
			this.Id = Guid.NewGuid();
			this.Name = name;
			this.PrimaryStateRegistrationNumber = primaryStateRegistrationNumber;
			this.TaxpayerIdentificationNumber = taxpayerIdentificationNumber;
			this.AuthorizedCapital = authorizedCapital;
			this.DirectorName = directorName;
			this.RegistrationDate = registrationDate;
			this.SmallBusinessSubjectAudits = new List<SmallBusinessSubjectAudit>();
		}

		public SmallBusinessSubject() {
			this.Id = Guid.NewGuid();
			this.SmallBusinessSubjectAudits = new List<SmallBusinessSubjectAudit>();
		}
	}
}
