namespace Muonroi.BaseTemplate.API.Application.Commands.Notification
{
    public class CreateNotificationCommandHandler(INotificationRepository repository)
        : IRequestHandler<CreateNotificationCommand, MResponse<NotificationDto>>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<MResponse<NotificationDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            NotificationEntity entity = new()
            {
                Icon = request.Icon,
                Url = request.Url,
                Action = request.Action,
                Title = request.Title,
                Description = request.Description,
                IsRead = false,
                Time = DateTime.UtcNow
            };
            _repository.Add(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            NotificationDto dto = new()
            {
                Id = entity.Id,
                Icon = entity.Icon,
                Url = entity.Url,
                Action = entity.Action,
                Title = entity.Title,
                Description = entity.Description,
                IsRead = entity.IsRead,
                Time = entity.Time
            };
            return new MResponse<NotificationDto> { Result = dto };
        }
    }
}
