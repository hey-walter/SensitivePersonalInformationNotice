
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Business.Services.Interfaces;
using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeResume.Business.Services
{
	public class TodoItemService : ITodoItemService
	{
		private readonly ITodoItemRepository _todoItemRepo;

		public TodoItemService(ITodoItemRepository todoItemRepo)
		{
			_todoItemRepo = todoItemRepo;
		}

		public IList<TodoItem> GetAll()
		{
			return _todoItemRepo.GetAll().OrderBy(x => x.TodoItemId).ToList();
		}

		public void AddTodoItems(IList<TodoItem> todoItems)
		{
			_todoItemRepo.BulkInsert(todoItems);
			DeleteExistedTodoItems();
		}

		public void Remove(TodoItem todoItem)
		{
			_todoItemRepo.Delete(todoItem);
		}

		public void DeleteExistedTodoItems()
		{
			_todoItemRepo.DeleteExistedTodoItems();
		}
	}
}
