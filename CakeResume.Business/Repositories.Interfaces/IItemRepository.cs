using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Business.Repositories.Interfaces
{
	public interface IItemRepository : IRepository<Item>
	{
		void UpdateNotYetVisitedItems();
	}
}
