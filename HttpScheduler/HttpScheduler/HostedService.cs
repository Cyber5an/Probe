using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Scheduler
{
	class HostedService : IHostedService
	{
		#region constructor
		private readonly ILogger<HostedService> logger;
		private readonly ILogic logic;
		private readonly AppSettings appSettings;

		public HostedService(ILogger<HostedService> logger, IOptions<AppSettings> options, ILogic logic)
		{
			this.logger = logger;
			this.logic = logic;
			this.appSettings = options.Value;
		}
		#endregion

		/// <summary>
		/// Przeniesione tutaj z Program.cs zeby testy mogly latwiej siegac do konfigurowania serwisow
		/// </summary>
		/// <param name="context"></param>
		/// <param name="services"></param>
		public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			services.Configure<AppSettings>(context.Configuration);
			services.AddHostedService<HostedService>();
			services.AddHttpClient();
			services.TryAddTransient<ILogic, Logic>();
		}

		/// <summary>
		/// Logger w tej metodzie jest domyslnie skonfigurowany tak, ze Information sie nie wyswietla, Error+ tak
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogTrace("Hosted service starting");
			try
			{
				await logic.RunAsync(cancellationToken);
			}
			catch (TaskCanceledException)
			{
				logger.LogInformation("Task cancelled exception");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "");
			}
		}
		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Hosted service stopping");

			return Task.CompletedTask;
		}
	}
}
