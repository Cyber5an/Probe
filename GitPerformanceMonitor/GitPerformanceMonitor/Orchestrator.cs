using System.Diagnostics;

namespace GitPerformanceMonitor;

public class Orchestrator : IOrchestrator
{
	#region constructor
	private readonly AppSettings appSettings;

	public Orchestrator(AppSettings appSettings)
	{
		this.appSettings = appSettings;
	}
	#endregion

	private static async Task<T> GoAsync<T>(Context context, Func<T, Task> func) where T : BaseReport, new()
	{
		var t = new T();
		try
		{
			await func(t);
		}
		catch (Exception ex)
		{
			t.ExceptionInfo = ExceptionInfo.FromException(ex);
		}
		finally
		{
			t.Stop = DateTime.Now;
		}
		return t;
	}

	private static ExternalCall ParseStringToCommand(string cmd)
	{
		var tokens = cmd.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (tokens.Length == 0) throw new Exception($"Parsing command failed for {cmd}");
		return new ExternalCall(tokens[0], tokens[1..]);
	}

	private static async Task<ReportCommand> RunCommandAsync(Context context, string command) => await GoAsync<ReportCommand>(context, async (outcome) =>
	{
		outcome.Name = command;
		var externalCall = ParseStringToCommand(command);
		var startInfo = new ProcessStartInfo
		{
			CreateNoWindow = false,
			FileName = externalCall.Executable,
			WorkingDirectory = context.WorkingDirectory
		};

		foreach (var arg in externalCall.Arguments)
		{
			startInfo.ArgumentList.Add(arg);
		}

		var process = Process.Start(startInfo) ?? throw new Exception($"Process started is null {startInfo.WorkingDirectory} {startInfo.FileName} {externalCall.JoinedArgs}");

		await process.WaitForExitAsync(cancellationToken: context.CancellationToken);
		outcome.ExitCode = process.ExitCode;
	});

	private async Task<ReportGroup> RunGroupAsync(Context context, Group group) => await GoAsync<ReportGroup>(context, async (outcome) =>
	{
		outcome.Name = group.Name;
		foreach (var command in group.Commands)
		{
			outcome.ReportItems.Add(await RunCommandAsync(context, command));
		}
	});

	private async Task<ReportSuite> RunSuiteAsync(Context context, Suite suite) => await GoAsync<ReportSuite>(context, async (outcome) =>
	{
		outcome.Name = suite.Name;
		foreach (var group in suite.Groups)
		{
			outcome.ReportGroups.Add(await RunGroupAsync(context, group));
		}
	});

	public async Task<ReportSuite> RunAsync(string suiteName, CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(appSettings.WorkingDirectory);
		var context = new Context { CancellationToken = cancellationToken };
		var suite = appSettings.Suites.Single(suite => suite.Name == suiteName);
		return await RunSuiteAsync(context, suite);
	}
}
