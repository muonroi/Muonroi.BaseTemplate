

namespace Muonroi.BaseTemplate.Data.Persistence
{
    public class BaseTemplateDbContext : MDbContext
    {
        public BaseTemplateDbContext(DbContextOptions options)
        : base(options, new NoMediator())
        { }

        public BaseTemplateDbContext
            (DbContextOptions options, IMediator mediator) : base(options, mediator)
        { }

    }
}
