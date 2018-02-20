using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CakeResume.Business.Services.Interfaces
{
	public interface ISearchService
	{
		IEnumerable<string> GetItemNames(
			int page, string country = null, string work_experience = null, string job_search_progress = null, string query = null);
		IList<string> GetAllItemNames();
		IList<string> GetItemNamesByKeyword(string keyword);
	}
}
