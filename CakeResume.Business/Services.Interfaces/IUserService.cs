using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Business.Services.Interfaces
{
	public interface IUserService
	{
		void AddUserWithItem(User user);
	}
}
