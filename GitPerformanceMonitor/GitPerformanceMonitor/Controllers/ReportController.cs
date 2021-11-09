using Microsoft.AspNetCore.Mvc;

namespace GitPerformanceMonitor.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
	#region constructor
	private readonly IOrchestrator orchestrator;

	public ReportController(IOrchestrator orchestrator)
	{
		this.orchestrator = orchestrator;
	}
	#endregion

	[HttpGet(nameof(GetReportSuite))]
	[ProducesResponseType(typeof(ReportSuite), 200)]
	public async Task<ActionResult<ReportSuite>> GetReportSuite(string suiteName)
	{
		try
		{
			return await orchestrator.RunAsync(suiteName, CancellationToken.None);
		}
		catch (Exception ex)
		{
			return BadRequest(ExceptionInfo.FromException(ex));
		}
	}
}
