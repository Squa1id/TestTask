using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TestTask.Data.Entities;

namespace TestTask.Data {
	public sealed class Context : DbContext {
		public DbSet<SmallBusinessSubject> SmallBusinessSubjects { get; set; }
		public DbSet<SmallBusinessSubjectAudit> SmallBusinessSubjectAudits { get; set; }
		public DbSet<SupervisoryAuthority> SupervisoryAuthorities { get; set; }
		
		public Context(DbContextOptions<Context> options) : base(options) {
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			#region SmallBusinessSubjects
			modelBuilder.Entity<SmallBusinessSubject>().HasKey(x => x.Id);
			modelBuilder.Entity<SmallBusinessSubject>().Property(x => x.PrimaryStateRegistrationNumber)
				.IsRequired()
				.HasMaxLength(13);
			modelBuilder.Entity<SmallBusinessSubject>().Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(256);
			modelBuilder.Entity<SmallBusinessSubject>().Property(x => x.TaxpayerIdentificationNumber)
				.IsRequired()
				.HasMaxLength(10);
			modelBuilder.Entity<SmallBusinessSubject>().Property(x => x.DirectorName)
				.HasMaxLength(128);
			#endregion

			#region SupervisoryAuthority
			modelBuilder.Entity<SupervisoryAuthority>().HasKey(x => x.Id);
			modelBuilder.Entity<SmallBusinessSubject>().Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(256);
			#endregion

			#region SmallBusinessSubjectAudit
			modelBuilder.Entity<SmallBusinessSubjectAudit>().HasKey(x => x.Id);

			modelBuilder.Entity<SmallBusinessSubjectAudit>().Property(x => x.BeginAuditDate)
				.IsRequired();
			modelBuilder.Entity<SmallBusinessSubjectAudit>().Property(x => x.EndAuditDate)
				.IsRequired();
			modelBuilder.Entity<SmallBusinessSubjectAudit>().Property(x => x.AuditDuration)
				.IsRequired();

			modelBuilder.Entity<SmallBusinessSubjectAudit>().HasOne(x => x.SmallBusinessSubject)
				.WithMany(x => x.SmallBusinessSubjectAudits)
				.HasForeignKey(x => x.SmallBusinessSubjectId);

			modelBuilder.Entity<SmallBusinessSubjectAudit>().HasOne(x => x.SupervisoryAuthority)
				.WithMany(x => x.SmallBusinessSubjectAudits)
				.HasForeignKey(x => x.SupervisoryAuthorityId);
			#endregion

			#region Тестовые данные
			var smallBusinessSubjects = new[] {
				new SmallBusinessSubject("ООО 'ИНТЕРНЕТ-ФРЕГАТ'", "1026102223608","6150032475", 16800, "Хомяков Сергей Валентинович", new DateTime(2000, 10,9)),
				new SmallBusinessSubject("ООО 'ИНТЕГРАТОР'", "1026103719091","6165001945", 10000, "Гущина Любовь Борисовна", new DateTime(1993, 2,24)),
				new SmallBusinessSubject("ООО 'СОФТДОН'", "1026104146023","6167009019", 10000, "Кузанова Маргарита Петровна", new DateTime(1991, 5,25)),
			};
			var supervisoryAuthorities = new[] {
				new SupervisoryAuthority("Роскомнадзор"),
				new SupervisoryAuthority("Роспотребнадзор"),
				new SupervisoryAuthority("СЭС")
			};

			modelBuilder.Entity<SmallBusinessSubject>().HasData(smallBusinessSubjects);
			modelBuilder.Entity<SupervisoryAuthority>().HasData(supervisoryAuthorities);

			#endregion

			base.OnModelCreating(modelBuilder);
		}
	}
}
