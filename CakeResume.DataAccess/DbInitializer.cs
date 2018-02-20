using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.DataAccess
{
	public static class DbInitializer
	{
		public static void SetInitializer(CakeResumeDbContext db)
		{
			if (db.Database.EnsureCreated())
			{

			}
		}
	}
}
