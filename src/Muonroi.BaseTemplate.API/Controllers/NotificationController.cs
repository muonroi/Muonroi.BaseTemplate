namespace Muonroi.BaseTemplate.API.Controllers
{
    public class NotificationController(IMediator mediator, ILogger logger, IMapper mapper) : MControllerBase(mediator, logger, mapper)
    {
        [HttpPost]
        [Permission<Permission>(Permission.Notification_Create)]
        public async Task<IActionResult> Create([FromBody] CreateNotificationCommand command, CancellationToken cancellationToken)
        {
            MResponse<NotificationDto> result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }

        [HttpGet]
        [Permission<Permission>(Permission.Notification_GetAll)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            MResponse<IEnumerable<NotificationDto>> result = await Mediator.Send(new GetNotificationsQuery(), cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }
    }
}
