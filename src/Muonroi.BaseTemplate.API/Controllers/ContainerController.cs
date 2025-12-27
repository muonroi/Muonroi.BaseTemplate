namespace Muonroi.BaseTemplate.API.Controllers;

public class ContainerController(IMediator mediator, ILogger logger, IMapper mapper) : MControllerBase(mediator, logger, mapper)
{
    [HttpPost]
    [Permission<Permission>(Permission.Container_Create)]
    public async Task<IActionResult> Create([FromBody] CreateContainerCommand command, CancellationToken cancellationToken)
    {
        MResponse<string> result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return result.GetActionResult();
    }
}
