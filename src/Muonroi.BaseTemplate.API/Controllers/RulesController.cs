namespace Muonroi.BaseTemplate.API.Controllers;

public class RulesController(IMediator mediator, ILogger logger, IMapper mapper)
    : MControllerBase(mediator, logger, mapper)
{
    [HttpGet("evaluate/{value:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Evaluate(int value, CancellationToken cancellationToken)
    {
        MResponse<FactBag> response = await Mediator
            .Send(new EvaluateNumberQuery(value), cancellationToken)
            .ConfigureAwait(false);
        return response.GetActionResult();
    }
}
