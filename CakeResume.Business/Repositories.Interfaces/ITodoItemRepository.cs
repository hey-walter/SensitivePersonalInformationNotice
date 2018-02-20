using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CakeResume.Models;

namespace CakeResume.Business.Repositories.Interfaces
{
	public interface ITodoItemRepository : IRepository<TodoItem>
	{
		void DeleteExistedTodoItems();
	}
}
