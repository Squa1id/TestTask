using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using TestTask.Data.Interfaces;

namespace TestTask.Data.Repositories {
	public abstract class BaseRepository<TModel, TViewModel>
		where TViewModel : class
		where TModel : class {
		protected readonly Context _context;

		protected BaseRepository(Context context) {
			this._context = context;
		}

		/// <summary>
		/// Преобразовывает сущность из базы данных в модель представления
		/// </summary>
		public abstract Func<TModel, TViewModel> ModelToViewModel();

		public IQueryable<FModel> GetEntities<FModel>(bool isNoTracking = false, params Expression<Func<FModel, object>>[] includes)
			where FModel : class, TModel {
			var query = _context.Set<FModel>().AsQueryable();
			if (includes != null && includes.Length > 0) {
				foreach (var include in includes) {
					query = query.Include(include);
				}
			}
			if (isNoTracking) {
				return query.AsNoTracking();
			}
			return query;
		}

		public virtual TEntity GetEntityById<TEntity>(Guid id, bool isNoTracking = false, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : class, TModel, IKeyEntity {
			return GetEntities(isNoTracking, includes).FirstOrDefault(x => x.Id == id);
		}
		public virtual TViewModel GetEntityViewModelById<TEntity>(Guid id, bool isNoTracking = false, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : class, TModel, IKeyEntity {
			return GetEntities(isNoTracking, includes).Where(x => x.Id == id).Select(ModelToViewModel()).FirstOrDefault();
		}

		public virtual void DeleteEntityById<TEntity>(Guid id)
			where TEntity : class, TModel, IKeyEntity {
			_context.Set<TEntity>().Remove(_context.Set<TEntity>().First(x => x.Id == id));
			_context.SaveChanges();
		}
	}
}
