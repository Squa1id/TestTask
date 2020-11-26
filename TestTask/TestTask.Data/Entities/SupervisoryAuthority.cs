using System;
using System.Collections.Generic;
using System.Text;
using TestTask.Data.Interfaces;

namespace TestTask.Data.Entities
{
	/// <summary>
	/// Контролирующий орган
	/// </summary>
	public class SupervisoryAuthority: IKeyEntity, INameEntity
	{
		public Guid Id { get; set; }
		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }
		
		public ICollection<SmallBusinessSubjectAudit> SmallBusinessSubjectAudits { get; set; }

		public SupervisoryAuthority(string name)
		{
			this.Id = Guid.NewGuid();
			this.Name = name;
			this.SmallBusinessSubjectAudits = new List<SmallBusinessSubjectAudit>();
		}

		public SupervisoryAuthority() {
			this.Id = Guid.NewGuid();
			this.SmallBusinessSubjectAudits = new List<SmallBusinessSubjectAudit>();
		}
	}
}
