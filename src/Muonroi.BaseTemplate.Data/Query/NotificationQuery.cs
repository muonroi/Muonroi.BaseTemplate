namespace Muonroi.BaseTemplate.Data.Query
{
    public class NotificationQuery(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext)
        : MQuery<NotificationEntity>(dbContext, authContext), INotificationQuery
    {
    }
}
