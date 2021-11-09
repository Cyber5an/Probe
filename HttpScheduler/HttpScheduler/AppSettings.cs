namespace Scheduler
{
	public class AppSettings
	{
		public string Version { get; set; } = "Not configured";
		public int DelayBetweenUrls { get; set; } = 30;
		public int DelayBetweenCalls { get; set; } = 1800;
		public string[] Urls { get; set; } = Array.Empty<string>();
	}
}
