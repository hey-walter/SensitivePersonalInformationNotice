
using CakeResume.Business.Options;
using CakeResume.Business.Services;
using CakeResume.Business.Services.Interfaces;
using CakeResume.DataAccess;
using CakeResume.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SensitivePersonalInformationNotice
{
	class Program
	{
		static IConfigurationRoot Configuration { get; set; }
		static void Main(string[] args)
		{
			Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var serviceProvider = ConfigureServices();

			serviceProvider
				.GetService<ILoggerFactory>()
				.AddConsole(LogLevel.Debug);

			var logger = serviceProvider.GetService<ILoggerFactory>()
				.CreateLogger<Program>();

			SetupDatabase(serviceProvider);

			logger.LogDebug("Starting Crawler");

			var crawlerService = serviceProvider.GetService<ICrawlerService>();
			crawlerService.Run();

			var securityDetectService = serviceProvider.GetService<ISecurityDetectService>();
			securityDetectService.Run();

			logger.LogDebug("Finish");
		}

		private static ServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection()
				.AddLogging();

			var assemblys = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblys.Where(x => x.FullName.StartsWith("CakeResume.")))
			{
				var types =
					assembly
					.DefinedTypes
					.Where(t => t.IsClass)
					.Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"));

				foreach (var implementationType in types)
				{
					foreach (var serviceType in implementationType.GetInterfaces())
					{
						services.AddTransient(serviceType, implementationType);
					}
				}
			}

			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContextPool<CakeResumeDbContext>(options =>
			{
				options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true);
			});

			services.Configure<SecurityDetectMailConfig>(Configuration.GetSection("SecurityDetectMailConfig"));
			services.PostConfigure<SecurityDetectMailConfig>(options =>
			{
				if (string.IsNullOrEmpty(options.Body) &&
					!string.IsNullOrEmpty(options.BodyPath) &&
					File.Exists(options.BodyPath))
				{
					options.Body = File.ReadAllText(options.BodyPath, Encoding.UTF8);
				}
			});

			var serviceProvider = services.BuildServiceProvider();
			return serviceProvider;
		}

		private static void SetupDatabase(IServiceProvider serviceProvider)
		{
			using (var serviceScope = serviceProvider.CreateScope())
			{
				using (var db = serviceScope.ServiceProvider.GetService<CakeResumeDbContext>())
				{
					DbInitializer.SetInitializer(db);
				}
			}
		}
	}
}
