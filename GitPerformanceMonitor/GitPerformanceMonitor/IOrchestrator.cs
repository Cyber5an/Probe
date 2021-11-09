namespace GitPerformanceMonitor
{
	public interface IOrchestrator
	{
		Task<ReportSuite> RunAsync(string suiteName, CancellationToken cancellationToken);
	}
}