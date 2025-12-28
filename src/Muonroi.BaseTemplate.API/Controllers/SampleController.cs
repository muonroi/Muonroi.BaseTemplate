



namespace Muonroi.BaseTemplate.API.Controllers
{
    public class SampleController(IMediator mediator, ILogger logger, IMapper mapper) : MControllerBase(mediator, logger, mapper)
    {
        [HttpPost]
        [Permission<Permission>(Permission.Sample_Create)]
        public async Task<IActionResult> Create([FromBody] CreateSampleCommand command, CancellationToken cancellationToken)
        {
            MResponse<SampleDto> result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }

        [HttpGet]
        [Permission<Permission>(Permission.Sample_GetAll)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            MResponse<IEnumerable<SampleDto>> result = await Mediator.Send(new GetSamplesQuery(), cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }
    }
}
