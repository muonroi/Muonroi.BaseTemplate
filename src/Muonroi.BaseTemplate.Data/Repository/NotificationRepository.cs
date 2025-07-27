namespace Muonroi.BaseTemplate.Data.Repository
{
    public class NotificationRepository(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext)
        : MRepository<NotificationEntity>(dbContext, authContext), INotificationRepository
    {
    }
}
