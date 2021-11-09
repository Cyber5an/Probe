namespace GitPerformanceMonitor
{
	internal class Context
	{
		public string WorkingDirectory { get; internal set; } = "";
		public CancellationToken CancellationToken { get; internal set; } = default;
	}
}