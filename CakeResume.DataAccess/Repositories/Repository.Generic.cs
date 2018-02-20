
using CakeResume.Business.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CakeResume.DataAccess.Repositories
{
	public class Repository<TEntity> : Repository, IRepository<TEntity>
		where TEntity : class
	{
		protected readonly DbSet<TEntity> _set;

		public Repository(CakeResumeDbContext db) : base(db)
		{
			_set = db.Set<TEntity>();
		}
		
		public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
		{
			return _set.FirstOrDefault(predicate);
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return _set;
		}

		public virtual IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
		{
			return _set.Where(predicate);
		}

		public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
		{
			return _set.Any(predicate);
		}

		public virtual void Insert(TEntity entity)
		{
			_set.Add(entity);
			SaveChanges();
		}

		public virtual void Update(TEntity entity)
		{
			var entry = _db.Entry(entity);
			if (entry.State != EntityState.Modified)
			{
				_set.Update(entity);
			}
			SaveChanges();
		}

		public virtual void Delete(TEntity entity)
		{
			_set.Remove(entity);
			SaveChanges();
		}

		public virtual void Delete(IEnumerable<TEntity> entities)
		{
			_set.RemoveRange(entities);
			SaveChanges();
		}

		public virtual void BulkInsert(IList<TEntity> entities)
		{
			_db.BulkInsert(entities);
		}

		public virtual void BulkUpdate(IList<TEntity> entities)
		{
			_db.BulkUpdate(entities);
		}

		public virtual void BulkInsertOrUpdate(IList<TEntity> entities)
		{
			_db.BulkInsertOrUpdate(entities);
		}


	}
}
