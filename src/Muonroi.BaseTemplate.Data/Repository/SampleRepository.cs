namespace Muonroi.BaseTemplate.Data.Repository
{
    public class SampleRepository(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext)
        : MRepository<SampleEntity>(dbContext, authContext), ISampleRepository
    {
    }
}
