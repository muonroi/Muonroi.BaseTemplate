using Muonroi.BaseTemplate.Core.Domain;
using Muonroi.BaseTemplate.Core.Interfaces.Repository;
using Muonroi.BaseTemplate.Data.Persistence;

namespace Muonroi.BaseTemplate.Data.Repository
{
    public class SampleRepository(BaseTemplateDbContext dbContext, MAuthenticateInfoContext authContext)
        : MRepository<SampleEntity>(dbContext, authContext), ISampleRepository
    {
    }
}
