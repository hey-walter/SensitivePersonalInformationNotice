using CakeResume.Business.Options;
using CakeResume.Business.Repositories.Interfaces;
using CakeResume.Business.Services.Interfaces;
using CakeResume.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CakeResume.Business.Services
{
	public class SecurityDetectService : ISecurityDetectService
	{
		private readonly SecurityDetectMailConfig _mailConfig;
		private readonly ILogger<SecurityDetectService> _logger;
		private readonly IItemRepository _itemRepo;

		public SecurityDetectService(
			IOptions<SecurityDetectMailConfig> mailConfigAccessor,
			ILogger<SecurityDetectService> logger,
			IItemRepository itemRepo)
		{
			_mailConfig = mailConfigAccessor.Value;
			_logger = logger;
			_itemRepo = itemRepo;
		}

		public IList<string> GetSecurityKeywords()
		{
			throw new NotImplementedException();
		}

		public void Run()
		{
			// 優先通知含有嚴重個資的User
			var items =
				_itemRepo
				.SearchFor(x =>
					x.LastSendEmailsTime == null &&
					(
					x.ItemJson.Contains("身分證") ||
					x.ItemJson.Contains("地址") ||
					x.ItemJson.Contains("住址") ||
					x.ItemJson.Contains("生日") ||
					x.ItemJson.Contains("出生") ||
					x.ItemJson.Contains("籍貫") ||
					x.ItemJson.Contains("聯絡") ||
					x.ItemJson.Contains("電話") ||
					x.ItemJson.Contains("手機")
					))
				.OrderBy(x => x.ItemId)
				.Take(100)
				.ToList();

			if (!items.Any())
			{
				items =
					_itemRepo
					.SearchFor(x => x.LastSendEmailsTime == null)
					.OrderBy(x => x.ItemId)
					.Take(100)
					.ToList();
			}

			if (!items.Any())
			{
				return;
			}

			foreach (var item in items)
			{
				var emailPattern = @"([a-zA-Z0-9_.+-]{4,30})@([a-zA-Z0-9-]{2,30})((\.([a-zA-Z0-9-.]){2,3})+)";
				var emailMatches = Regex.Matches(item.ItemJson, emailPattern);
				var emails = emailMatches.Cast<Match>()
						.Select(m => m.Value)
						.Where(m => !m.Contains("@example.com") && !m.EndsWith(".png"))
						.Distinct()
						.ToList();

				var nameMatch = Regex.Match(item.ItemJson, @"""user"": \{.+?""name"": ""(.+?)""", RegexOptions.Singleline);
				var recipientName = nameMatch.Groups[1].Value;

				try
				{
					SendMail(_mailConfig, recipientName, emails);
					item.SendEmails = string.Join("; ", emails);
				}
				catch (SmtpFailedRecipientException) { }
				catch (SmtpException) { throw; }
				catch (IOException) { throw; }
				catch (Exception) { }

				item.LastSendEmailsTime = DateTime.Now;
				_itemRepo.Update(item);

				_logger.LogInformation(item.SendEmails);
			}

			Run();
		}

		private void SendMail(SecurityDetectMailConfig config, string recipientName, List<string> mailList)
		{
			using (var mail = new MailMessage())
			using (var stmp = new SmtpClient(config.Host, config.Port))
			{
				foreach (var address in mailList.Select(a => new MailAddress(a)))
				{
					mail.To.Add(address);
				}

				mail.From = new MailAddress(config.FromAddress, config.FromDisplayName, Encoding.UTF8);
				mail.Subject = string.Format(config.Subject, recipientName);
				mail.SubjectEncoding = Encoding.UTF8;
				mail.Body = config.Body;
				mail.IsBodyHtml = true;
				mail.BodyEncoding = Encoding.UTF8;
				mail.Priority = MailPriority.High;

				stmp.Credentials = new NetworkCredential(config.User, config.Password);
				stmp.EnableSsl = true;
				stmp.Send(mail);
			}
		}

	}
}
