using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CakeResume.Business.Helpers
{
	public static class HttpHelper
	{
		public static HttpResult<T> Get<T>(string url) where T : class
		{
			var http = new HttpClient();
			var response = http.GetAsync(url).GetAwaiter().GetResult();

			T obj = default(T);
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

				if (typeof(T) == typeof(string))
				{
					obj = content as T;
				}
				else
				{
					obj = JsonConvert.DeserializeObject<T>(content);
				}
			}

			return new HttpResult<T>()
			{
				IsSuccessStatusCode = response.IsSuccessStatusCode,
				StatusCode = response.StatusCode,
				Content = obj
			};
		}
	}

	public class HttpResult<T>
	{
		public bool IsSuccessStatusCode { get; internal set; }
		public HttpStatusCode StatusCode { get; internal set; }
		public T Content { get; internal set; }
	}
}
