using CakeResume.Business.Helpers;
using CakeResume.Business.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CakeResume.Business.Services
{
	/// <summary>
	/// 找人才服務
	/// </summary>
	public class SearchService : ISearchService
	{
		/// <summary>
		/// 所在地  
		/// 鄰近	"TW", "IN", "EG", "US", "CN", "SA", "CA", "KR", "MY", "TR"
		/// 全部	"AF",  "AX",  "AL",  "DZ",  "AS",  "AD",  "AO",  "AI",  "AQ",  "AG",  "AR",  "AM",  "AW",  "AU",  "AT",  "AZ",  "BS",  "BH",  "BD",  "BB",  "BY",  "BE",  "BZ",  "BJ",  "BM",  "BT",  "BO",  "BA",  "BW",  "BV",  "BR",  "IO",  "BN",  "BG",  "BF",  "BI",  "KH",  "CM",  "CA",  "CV",  "KY",  "CF",  "TD",  "CL",  "CN",  "CX",  "CC",  "CO",  "KM",  "CG",  "CD",  "CK",  "CR",  "CI",  "HR",  "CU",  "CY",  "CZ",  "DK",  "DJ",  "DM",  "DO",  "EC",  "EG",  "SV",  "GQ",  "ER",  "EE",  "ET",  "FK",  "FO",  "FJ",  "FI",  "FR",  "GF",  "PF",  "TF",  "GA",  "GM",  "GE",  "DE",  "GH",  "GI",  "GR",  "GL",  "GD",  "GP",  "GU",  "GT",  "GG",  "GN",  "GW",  "GY",  "HT",  "HM",  "VA",  "HN",  "HK",  "HU",  "IS",  "IN",  "ID",  "IR",  "IQ",  "IE",  "IM",  "IL",  "IT",  "JM",  "JP",  "JE",  "JO",  "KZ",  "KE",  "KI",  "KR",  "KW",  "KG",  "LA",  "LV",  "LB",  "LS",  "LR",  "LY",  "LI",  "LT",  "LU",  "MO",  "MK",  "MG",  "MW",  "MY",  "MV",  "ML",  "MT",  "MH",  "MQ",  "MR",  "MU",  "YT",  "MX",  "FM",  "MD",  "MC",  "MN",  "ME",  "MS",  "MA",  "MZ",  "MM",  "NA",  "NR",  "NP",  "NL",  "AN",  "NC",  "NZ",  "NI",  "NE",  "NG",  "NU",  "NF",  "MP",  "NO",  "OM",  "PK",  "PW",  "PS",  "PA",  "PG",  "PY",  "PE",  "PH",  "PN",  "PL",  "PT",  "PR",  "QA",  "RE",  "RO",  "RU",  "RW",  "BL",  "SH",  "KN",  "LC",  "MF",  "PM",  "VC",  "WS",  "SM",  "ST",  "SA",  "SN",  "RS",  "SC",  "SL",  "SG",  "SK",  "SI",  "SB",  "SO",  "ZA",  "GS",  "ES",  "LK",  "SD",  "SR",  "SJ",  "SZ",  "SE",  "CH",  "SY",  "TW",  "TJ",  "TZ",  "TH",  "TL",  "TG",  "TK",  "TO",  "TT",  "TN",  "TR",  "TM",  "TC",  "TV",  "UG",  "UA",  "AE",  "GB",  "US",  "UM",  "UY",  "UZ",  "VU",  "VE",  "VN",  "VG",  "VI",  "WF",  "EH",  "YE",  "ZM",  "ZW"
		/// </summary>
		private readonly string[] IsoCountries = { "TW", "IN", "EG", "US", "CN", "SA", "CA", "KR", "MY", "TR" };
		/// <summary>
		/// 工作經驗
		/// </summary>
		private readonly string[] WorkExperience = { "one_two", "two_four", "four_six", "six_ten", "ten_fifteen", "fifteen_" };
		/// <summary>
		/// 求職階段
		/// </summary>
		private readonly string[] JobSearchProgress = { "not_open_to_opportunities", "open_to_opportunities", "ready_to_interview", "interviewing_early_stages", "interviewing_final_stages", "have_offsers" };

		private readonly ILogger<SearchService> _logger;

		public SearchService(ILogger<SearchService> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// 取得履歷項目名稱
		/// </summary>
		/// <param name="page"></param>
		/// <param name="country"></param>
		/// <param name="work_experience"></param>
		/// <param name="job_search_progress"></param>
		/// <returns></returns>
		public IEnumerable<string> GetItemNames(
			int page, string country = null, string work_experience = null, string job_search_progress = null, string query = null)
		{
			var parms = new Dictionary<string, object>();
			parms["page"] = page;

			if (!string.IsNullOrWhiteSpace(country))
				parms["refinementList%5Buser.country%5D%5B0%5D"] = country;

			if (!string.IsNullOrWhiteSpace(work_experience))
				parms["refinementList%5Buser.work_experience%5D%5B0%5D"] = work_experience;

			if (!string.IsNullOrWhiteSpace(job_search_progress))
				parms["refinementList%5Buser.job_search_progress%5D%5B0%5D"] = job_search_progress;

			if (!string.IsNullOrWhiteSpace(query))
				parms["query"] = query;

			var url = $"https://www.cakeresume.com/search?" + string.Join("&", parms.Select(x => $"{x.Key}={x.Value}"));
			var result = HttpHelper.Get<string>(url);
			if (!result.IsSuccessStatusCode)
			{
				return Enumerable.Empty<string>();
			}

			var matches = Regex.Matches(result.Content, @"""user-name link-propagate"" href=""/search/(.+?)""");
			var items = matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
			return items;
		}

		/// <summary>
		/// 取得所有履歷項目名稱
		/// </summary>
		/// <returns></returns>
		public IList<string> GetAllItemNames()
		{
			var pageItemNames = new List<string>();

			foreach (var isoCountry in IsoCountries)
			{
				// [加快搜尋]依照該國家是否超過公開一千筆履歷(1000/15=66.7)
				if ((GetItemNames(67, isoCountry)).Any())
				{
					foreach (var workExp in WorkExperience)
					{
						foreach (var jobSearchProgres in JobSearchProgress)
						{
							GetItemNamesByEachPages(pageItemNames, isoCountry, workExp, jobSearchProgres);
						}
					}
				}
				else
				{
					GetItemNamesByEachPages(pageItemNames, isoCountry);
				}
			}

			return pageItemNames;
		}


		/// <summary>
		/// 依照條件從第一頁搜尋到最後一頁
		/// </summary>
		/// <param name="pageItemNames"></param>
		/// <param name="isoCountry"></param>
		/// <param name="workExp"></param>
		/// <param name="jobSearchProgres"></param>
		/// <returns></returns>
		private void GetItemNamesByEachPages(
			List<string> pageItemNames, string isoCountry = null, string workExp = null, string jobSearchProgres = null, string query = null)
		{
			int page = 1;
			IEnumerable<string> itemNames = null;
			do
			{
				_logger.LogInformation("Search Page:{0}, IsoCountry:{1}, WorkExperience:{2}, JobSearchProgress:{3}, query:{4}",
										page, isoCountry, workExp, jobSearchProgres, query);

				itemNames = GetItemNames(page, isoCountry, workExp, jobSearchProgres, query);
				pageItemNames.AddRange(itemNames);
				page++;
			} while (itemNames.Any());
		}

		public IList<string> GetItemNamesByKeyword(string keyword)
		{
			var pageItemNames = new List<string>();

			if ((GetItemNames(67, query: keyword)).Any())
			{
				foreach (var workExp in WorkExperience)
				{
					GetItemNamesByEachPages(pageItemNames, workExp: workExp, query: keyword);
				}
			}
			else
			{
				GetItemNamesByEachPages(pageItemNames, query: keyword);
			}

			return pageItemNames;
		}
	}
}
