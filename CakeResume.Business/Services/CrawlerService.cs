
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Business.Services.Interfaces;
using CakeResume.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CakeResume.Business.Services
{
	public class CrawlerService : ICrawlerService
	{
		private readonly ILogger<CrawlerService> _logger;
		private readonly ITodoItemService _todoItemService;
		private readonly ISearchService _searchService;
		private readonly IItemService _itemService;
		private readonly IUserService _userService;
		private readonly IItemRepository _itemRepo;

		public CrawlerService(
			ILogger<CrawlerService> logger,
			ITodoItemService todoItemService,
			ISearchService searchService,
			IItemService itemService,
			IUserService userService,
			IItemRepository itemRepo)
		{
			_logger = logger;
			_todoItemService = todoItemService;
			_searchService = searchService;
			_itemService = itemService;
			_userService = userService;
			_itemRepo = itemRepo;
		}

		public void Run()
		{
			var crawlSearchPagesResult = CrawlSearchPages();

			var crawSimilaritiesResult = CrawSimilarities();

			if (crawlSearchPagesResult || crawSimilaritiesResult)
			{
				Run();
			}
		}

		/// <summary>
		/// 找出不應出現在公開履歷中的關鍵字
		/// </summary>
		private void SearchKeywords()
		{
			var keywords = new[] { "身分證", "地址", "住址", "生日", "出生", "籍貫", "聯絡", "電話", "手機", "姓名", "血型", "體重", "年齡", "身高", "Birthday", "Birth", "Identity", "Card", "ID", "Card", "Gender", "Blood", "Type", "Blood", "type", "MAIL", "Email", "email", "Phone Number", "Address", "Born", "Hight", "Weight", "Location", "Language", "Contact", "Mobile", "Passport", "性别", "学位", "电话", "专业学历", "政治面貌", "籍贯", "手机", "年龄", "现居地" };
			foreach (var keyword in keywords)
			{
				var itemIds = _searchService.GetItemNamesByKeyword(keyword);

				_todoItemService.AddTodoItems(itemIds.Select(x => new TodoItem { ItemId = x }).ToList());
			}
		}

		/// <summary>
		/// 爬取搜尋頁面
		/// </summary>
		/// <returns></returns>
		private bool CrawlSearchPages()
		{
			_todoItemService.DeleteExistedTodoItems();

			// 檢查上次是否有爬到一半的待辦項目
			var todoItems = _todoItemService.GetAll();
			if (!todoItems.Any())
			{
				SearchKeywords();

				// 取得搜尋頁面所有項目
				var itemIds = _searchService.GetAllItemNames();

				// 加入尚未處理的項目到待辦項目佇列中
				_todoItemService.AddTodoItems(itemIds.Select(x => new TodoItem { ItemId = x }).ToList());

				// 取得可處理的待辦項目
				todoItems = _todoItemService.GetAll();

				// 搜尋頁面已處理完則結束
				if (!todoItems.Any())
				{
					return false;
				}
			}

			foreach (var todoItem in todoItems)
			{
				try
				{
					// 爬取新的履歷項目
					var userWithItem = _itemService.GetUserWithItem(todoItem.ItemId);
					using (var ts = new TransactionScope())
					{
						if (userWithItem != null)
						{
							_userService.AddUserWithItem(userWithItem);
						}

						_todoItemService.Remove(todoItem);

						ts.Complete();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(30001, ex, $"TodoItem {todoItem.ItemId} Exception.");
				}
			}

			return true;
		}

		/// <summary>
		/// 爬取相似之處的履歷
		/// </summary>
		/// <returns></returns>
		private bool CrawSimilarities()
		{
			// 取得尚未訪問相似之處的項目
			var items = _itemService.GetNotYetVisitedItems();
			if (!items.Any())
			{
				return false;
			}

			foreach (var item in items)
			{
				try
				{
					// 取得這項目的相似之處履歷項目
					var similarities = _itemService.GetSimilarities(item.ItemId);

					using (var ts = new TransactionScope())
					{
						foreach (var userWithItem in similarities)
						{
							// 將尚未記錄過的履歷儲存起來
							var similaritiesItem = userWithItem.Items.FirstOrDefault();
							if (!_itemRepo.Exists(x => x.ItemId == similaritiesItem.ItemId))
							{
								_userService.AddUserWithItem(userWithItem);
							}
						}
						// 拜訪完成註記
						item.LastAccessSimilaritiesTime = DateTime.Now;
						_itemRepo.Update(item);

						ts.Complete();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(30001, ex, $"ItemId {item.ItemId} Exception.");
				}
			}

			return true;
		}
	}

}
