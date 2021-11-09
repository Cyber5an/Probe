using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Scheduler
{
	class Logic : ILogic
	{
		#region constructor
		private readonly ILogger<Logic> logger;
		private readonly IHttpClientFactory httpClientFactory;
		private readonly AppSettings appSettings;

		public Logic(ILogger<Logic> logger, IOptions<AppSettings> options, IHttpClientFactory httpClientFactory)
		{
			this.logger = logger;
			this.httpClientFactory = httpClientFactory;
			this.appSettings = options.Value;
		} 
		#endregion

		public async Task RunAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation(appSettings.Version);
			await Task.Delay(1000, cancellationToken);
			var tasks = new List<Task>();
			foreach (var url in appSettings.Urls)
			{
				tasks.Add(Task.Run(() => Listening(url, cancellationToken), cancellationToken));
				await Task.Delay(TimeSpan.FromSeconds(appSettings.DelayBetweenUrls), cancellationToken);
			}
			await Task.WhenAll(tasks);
		}

		private async void Listening(string url, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled) throw new NotSupportedException("Cancellation token cannot be cancelled");
			var client = httpClientFactory.CreateClient(url);
			
			while (! cancellationToken.IsCancellationRequested)
			{
				//var now = DateTime.Now;
				try
				{
					var response = await client.GetStringAsync(url, cancellationToken);
					logger.LogInformation("{url}\t{response}", url, response);
				}
				catch (Exception ex)
				{
					logger.LogWarning("{url}\t{exceptionMessage}", url, ex.Message);
				}
				var delay = TimeSpan.FromSeconds(appSettings.DelayBetweenCalls);
				logger.LogDebug("Will delay {delay} seconds", delay);
				await Task.Delay(delay, cancellationToken);
			}
		}
	}
}
