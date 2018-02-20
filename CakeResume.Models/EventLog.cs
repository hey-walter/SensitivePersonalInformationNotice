using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Models
{
    public class EventLog
    {
		public int EventLogId { get; set; }
		public string Message { get; set; }
		public string Exception { get; set; }
		public DateTime CreatedTime { get; set; } = DateTime.Now;
		public DateTime? UpdatedTime { get; set; }

	}
}
