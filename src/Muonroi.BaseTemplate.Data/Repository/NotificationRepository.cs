namespace Muonroi.BaseTemplate.Data.Repository
{
    public class NotificationRepository(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext, ILicenseGuard licenseGuard)
        : MRepository<NotificationEntity>(dbContext, authContext, licenseGuard), INotificationRepository
    {
    }
}
