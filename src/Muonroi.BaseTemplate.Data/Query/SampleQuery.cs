using Muonroi.BaseTemplate.Core.Domain;
using Muonroi.BaseTemplate.Core.Interfaces.Query;
using Muonroi.BaseTemplate.Data.Persistence;

namespace Muonroi.BaseTemplate.Data.Query
{
    public class SampleQuery(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext)
    : MQuery<SampleEntity>(dbContext, authContext), ISampleQuery
    {
    }
}
