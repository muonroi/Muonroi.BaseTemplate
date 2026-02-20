namespace Muonroi.BaseTemplate.Data.Repository
{
    public class SampleRepository(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext, ILicenseGuard licenseGuard)
        : MRepository<SampleEntity>(dbContext, authContext, licenseGuard), ISampleRepository
    {
    }
}
