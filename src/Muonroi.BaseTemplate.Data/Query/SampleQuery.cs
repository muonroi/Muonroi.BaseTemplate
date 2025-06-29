namespace Muonroi.BaseTemplate.Data.Query
{
    public class SampleQuery(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext)
    : MQuery<SampleEntity>(dbContext, authContext), ISampleQuery
    {
    }
}
