



namespace Muonroi.BaseTemplate.Data.Query
{
    public class SampleQuery(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext, ILicenseGuard licenseGuard)
    : MQuery<SampleEntity>(dbContext, authContext, licenseGuard), ISampleQuery
    {
    }
}
