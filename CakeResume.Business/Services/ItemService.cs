using CakeResume.Business.Helpers;
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Business.Services.Interfaces;
using CakeResume.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeResume.Business.Services
{
	public class ItemService : IItemService
	{
		private readonly IItemRepository _itemRepo;

		public ItemService(IItemRepository itemRepo)
		{
			_itemRepo = itemRepo;
		}

		public User GetUserWithItem(string itemName)
		{
			var url = $"https://www.cakeresume.com/api/v1/items/{itemName}";
			var result = HttpHelper.Get<string>(url);

			if (result.Content == null)
				return null;

			var item = ParseUserWithItem(result.Content);
			return item;
		}


		public IEnumerable<User> GetSimilarities(string itemName)
		{
			var url = $"https://www.cakeresume.com/api/v1/items/{itemName}/similarities";
			var result = HttpHelper.Get<string>(url);
			if (!result.IsSuccessStatusCode)
			{
				return Enumerable.Empty<User>();
			}

			var users = ParseUserWithItems(result.Content);
			return users;
		}

		private List<User> ParseUserWithItems(string content)
		{
			var users = new List<User>();
			var jObject = JObject.Parse(content);
			foreach (JObject item in jObject.GetValue("items"))
			{
				var user = ParseUserWithItem(item);
				users.Add(user);
			}
			return users;
		}

		private User ParseUserWithItem(string content)
		{
			var jObject = JObject.Parse(content);
			return ParseUserWithItem(jObject.GetValue("item") as JObject);
		}
		private User ParseUserWithItem(JObject jObject)
		{
			dynamic root = jObject;
			string path = root.path;
			string name = root.user.name;
			string userName = root.user.username;
			string email = root.user.email;

			var user = new User
			{
				Name = name,
				UserName = userName,
				Email = email
			};

			user.Items.Add(new Item
			{
				ItemId = path,
				ItemJson = jObject.ToString()
			});

			return user;
		}
		
		public IList<Item> GetNotYetVisitedItems()
		{
			return _itemRepo.SearchFor(x => x.LastAccessSimilaritiesTime == null).ToList();
		}
	}
}
