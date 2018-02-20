using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CakeResume.Business.Services.Interfaces
{
    public interface ITodoItemService
    {
		IList<TodoItem> GetAll();
		void AddTodoItems(IList<TodoItem> todoItems);
		void Remove(TodoItem todoItem);
		void DeleteExistedTodoItems();
	}
}
