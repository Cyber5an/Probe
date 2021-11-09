using Newtonsoft.Json;

namespace GitPerformanceMonitor;

public class ReportCommand : BaseReport
{
	public int ExitCode { get; set; }
}
