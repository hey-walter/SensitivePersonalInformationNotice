
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace CakeResume.DataAccess.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(CakeResumeDbContext db) : base(db)
		{
		}

		public User Find(string email)
		{
			return _set.SingleOrDefault(x => x.Email == email);
		}
	}
}
