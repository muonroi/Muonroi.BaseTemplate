namespace Muonroi.BaseTemplate.API.Application.Commands.Notification
{
    public class CreateNotificationCommand : IRequest<MResponse<NotificationDto>>
    {
        public string Icon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
