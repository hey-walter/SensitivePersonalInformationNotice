using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CakeResume.Business.Repositories.Interfaces
{
	public interface IRepository<TEntity> : IRepository
	{
		TEntity Get(Expression<Func<TEntity, bool>> predicate);
		IQueryable<TEntity> GetAll();
		IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
		bool Exists(Expression<Func<TEntity, bool>> predicate);
		void Insert(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);
		void Delete(IEnumerable<TEntity> entities);
		void BulkInsert(IList<TEntity> entities);
		void BulkUpdate(IList<TEntity> entities);
		void BulkInsertOrUpdate(IList<TEntity> entities);
	}
}
