
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Business.Services.Interfaces;
using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CakeResume.Business.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepo;

		public UserService(IUserRepository userRepo)
		{
			_userRepo = userRepo;
		}

		public void AddUserWithItem(User userWithItem)
		{
			var user = _userRepo.Find(userWithItem.Email);
			if (user == null)
			{
				_userRepo.Insert(userWithItem);
			}
			else
			{
				var addItems = userWithItem.Items.Where(x => user.Items.All(y => y.ItemId != x.ItemId)).ToList();
				if (!addItems.Any())
				{

				}
				user.Items.AddRange(addItems);
				_userRepo.Update(user);
			}
		}
	}
}
