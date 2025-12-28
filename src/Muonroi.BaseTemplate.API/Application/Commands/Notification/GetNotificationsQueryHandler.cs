

namespace Muonroi.BaseTemplate.API.Application.Commands.Notification
{
    public class GetNotificationsQueryHandler(INotificationQuery query)
        : IRequestHandler<GetNotificationsQuery, MResponse<IEnumerable<NotificationDto>>>
    {
        private readonly INotificationQuery _query = query;

        public async Task<MResponse<IEnumerable<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            List<NotificationEntity>? entities = await _query.GetAllAsync().ConfigureAwait(false);
            IEnumerable<NotificationDto> dtos = entities?.Select(e => new NotificationDto
            {
                Id = e.Id,
                Icon = e.Icon,
                Url = e.Url,
                Action = e.Action,
                Title = e.Title,
                Description = e.Description,
                IsRead = e.IsRead,
                Time = e.Time
            }) ?? [];
            return new MResponse<IEnumerable<NotificationDto>> { Result = dtos };
        }
    }
}
