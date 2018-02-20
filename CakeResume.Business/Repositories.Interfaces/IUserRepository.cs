using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Business.Repositories.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		User Find(string userName);
	}
}
