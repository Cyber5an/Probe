namespace Scheduler
{
	internal interface ILogic
	{
		Task RunAsync(CancellationToken cancellationToken);
	}
}