using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Models
{
	public class Item
	{
		public string ItemId { get; set; }
		public string ItemJson { get; set; }
		public DateTime? LastAccessSimilaritiesTime { get; set; }
		public string SendEmails { get; set; }
		public DateTime? LastSendEmailsTime { get; set; }
		public DateTime CreatedTime { get; set; } = DateTime.Now;
		public DateTime? UpdatedTime { get; set; }
		
		public string Email { get; set; }
		public User User { get; set; }
		public List<ItemImage> Images { get; set; }
	}
}
