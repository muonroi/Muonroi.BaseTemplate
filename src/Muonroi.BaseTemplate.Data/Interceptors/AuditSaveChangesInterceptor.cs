


namespace Muonroi.BaseTemplate.Data.Interceptors;

public sealed class AuditSaveChangesInterceptor(ILogger<AuditSaveChangesInterceptor> logger) : SaveChangesInterceptor
{
    private readonly ILogger<AuditSaveChangesInterceptor> _logger = logger;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            _logger.LogInformation("Saving changes for {Context}", eventData.Context.GetType().Name);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            _logger.LogInformation("Saved {Count} entities for {Context}", result, eventData.Context.GetType().Name);
        }
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
