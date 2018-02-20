using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Models
{
    public class ItemImage
    {
		public Guid ItemImageId { get; set; }
		public string OriginalUrl { get; set; }
		public string SaveUrl { get; set; }
		public DateTime CreatedTime { get; set; }
		public DateTime? UpdatedTime { get; set; }

		public string ItemId { get; set; }
		public Item Item { get; set; }
	}
}
