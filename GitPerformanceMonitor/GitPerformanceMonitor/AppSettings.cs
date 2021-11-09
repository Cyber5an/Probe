namespace GitPerformanceMonitor;

public class AppSettings
{
	public string WorkingDirectory { get; set; } = "";
	public List<Suite> Suites { get; set; } = new();
}

public class Suite
{
	public string Name { get; set; } = "";
	public List<Group> Groups { get; set; } = new();
}

public class Group
{
	public string Name { get; set; } = "";
	public List<string> Commands { get; set; } = new();
}
