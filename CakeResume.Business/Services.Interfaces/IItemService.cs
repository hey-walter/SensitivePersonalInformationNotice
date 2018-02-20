using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CakeResume.Business.Services.Interfaces
{
	public interface IItemService
	{
		/// <summary>
		/// 取得履歷項目與使用者
		/// </summary>
		/// <param name="itemName"></param>
		/// <returns></returns>
		User GetUserWithItem(string itemName);
		/// <summary>
		/// 取得相似之處的項目
		/// </summary>
		/// <param name="itemName"></param>
		/// <returns></returns>
		IEnumerable<User> GetSimilarities(string itemName);

		/// <summary>
		/// 取得尚未訪問的項目
		/// </summary>
		/// <returns></returns>
		IList<Item> GetNotYetVisitedItems();
	}
}
