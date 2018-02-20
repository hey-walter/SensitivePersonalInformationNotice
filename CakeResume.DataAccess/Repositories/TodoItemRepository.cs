
using CakeResume.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using CakeResume.Business.Repositories.Interfaces;

namespace CakeResume.DataAccess.Repositories
{
	public class TodoItemRepository : Repository<TodoItem>, ITodoItemRepository
	{
		public TodoItemRepository(CakeResumeDbContext db) : base(db)
		{
		}

		public void DeleteExistedTodoItems()
		{
			var sql =
@"DELETE FROM TodoItems 
    WHERE TodoItems.ItemId IN 
    ( SELECT Items.ItemId FROM Items )";
			var result = Execute(sql);
		}
	}
}
