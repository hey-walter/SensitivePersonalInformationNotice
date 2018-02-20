using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Models
{
    public class User
    {
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public DateTime CreatedTime { get; set; } = DateTime.Now;
		public DateTime? UpdatedTime { get; set; }

		public List<Item> Items { get; set; } = new List<Item>();
	}
}
