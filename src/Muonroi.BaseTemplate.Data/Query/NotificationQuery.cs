namespace Muonroi.BaseTemplate.Data.Query
{
    public class NotificationQuery(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext, ILicenseGuard licenseGuard)
        : MQuery<NotificationEntity>(dbContext, authContext, licenseGuard), INotificationQuery
    {
    }
}
