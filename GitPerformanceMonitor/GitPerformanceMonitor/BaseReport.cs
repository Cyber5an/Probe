using Newtonsoft.Json;

namespace GitPerformanceMonitor;

public class BaseReport
{
	[JsonProperty (Order = -2)]
	public string Name { get; set; } = "";

	[JsonProperty(Order = -2)]
	public TimeSpan Time => Stop.HasValue ? Stop.Value - Start : TimeSpan.Zero;

	[JsonProperty (Order = -2)]
	public DateTime Start { get; init; } = DateTime.Now;

	[JsonProperty (Order = -2)]
	public DateTime? Stop { get; set; }

	[JsonProperty (Order = 2)]
	public ExceptionInfo? ExceptionInfo { get; set; }
}
