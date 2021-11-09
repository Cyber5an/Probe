using Newtonsoft.Json;

namespace GitPerformanceMonitor;

public class ReportGroup : BaseReport
{
	[JsonProperty(Order = 1)]
	public List<ReportCommand> ReportItems { get; set; } = new();
}
