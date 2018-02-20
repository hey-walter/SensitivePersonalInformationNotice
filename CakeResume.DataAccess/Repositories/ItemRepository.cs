
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.DataAccess.Repositories
{
	public class ItemRepository : Repository<Item>, IItemRepository
	{
		public ItemRepository(CakeResumeDbContext db) : base(db)
		{
		}

		public void UpdateNotYetVisitedItems()
		{
			var sql =
@"UPDATE Items
SET LastAccessSimilaritiesTime = GETDATE(), 
    UpdatedTime = GETDATE() 
WHERE LastAccessSimilaritiesTime IS NULL";

			var result = Execute(sql);
		}
	}
}
