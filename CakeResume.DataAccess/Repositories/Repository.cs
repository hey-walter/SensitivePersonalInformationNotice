using CakeResume.Business.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace CakeResume.DataAccess.Repositories
{
	public class Repository : IRepository
	{
		protected readonly CakeResumeDbContext _db;
		public Repository(CakeResumeDbContext db)
		{
			_db = db;
		}

		protected IDbConnection GetConnection()
		{
			return _db.Database.GetDbConnection();
		}

		protected int Execute(string sql, object param = null, IDbTransaction transaction = null)
		{
			var conn = GetConnection();
			return conn.Execute(sql, param, transaction);
		}

		protected int SaveChanges()
		{
			return _db.SaveChanges();
		}


	}
}
