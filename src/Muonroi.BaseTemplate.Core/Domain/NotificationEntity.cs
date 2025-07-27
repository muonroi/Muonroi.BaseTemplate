namespace Muonroi.BaseTemplate.Core.Domain
{
    public class NotificationEntity : MEntity
    {
        public string Icon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}
