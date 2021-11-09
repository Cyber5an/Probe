using Newtonsoft.Json;

namespace GitPerformanceMonitor;

public static class Extensions
{
	public static string ToJson<T>(this T source) => JsonConvert.SerializeObject(source);
}
