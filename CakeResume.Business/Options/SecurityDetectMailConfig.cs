using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Business.Options
{
    public class SecurityDetectMailConfig
    {
		public string Host { get; set; }
		public int Port { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string FromAddress { get; set; }
		public string FromDisplayName { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string BodyPath { get; set; }

	}
}
