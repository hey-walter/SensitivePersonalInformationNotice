using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Models
{
    public class SearchResult
    {
		public string SearchKey { get; set; }
		public int SearchCount { get; set; }
		public DateTime CreatedTime { get; set; } = DateTime.Now;
		public DateTime? UpdatedTime { get; set; }

	}
}
