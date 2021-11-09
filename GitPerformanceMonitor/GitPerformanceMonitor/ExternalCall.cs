namespace GitPerformanceMonitor;

class ExternalCall
{
	public ExternalCall(string executable, params string[] args)
	{
		this.Executable = executable;
		this.Arguments.AddRange(args);
		this.JoinedArgs = string.Join(' ', Arguments.Select(x => x.Trim()));
	}

	public string Executable { get; init; } = "";
	public List<string> Arguments { get; init; } = new();
	public string JoinedArgs { get; init; } = "";
}
