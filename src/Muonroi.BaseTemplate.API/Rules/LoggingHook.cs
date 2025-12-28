using Microsoft.Extensions.Logging;

namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("numbers")]
public sealed class LoggingHook(ILogger<LoggingHook> logger) : IHookHandler<int>
{
    private readonly ILogger<LoggingHook> _logger = logger;

    public Task HandleAsync(
        HookPoint point,
        IRule<int> rule,
        RuleResult result,
        FactBag facts,
        int context,
        TimeSpan? duration = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Rule {Rule} at {Point} in {Duration}ms with context {Context}: {Success}",
            rule.Name,
            point,
            duration?.TotalMilliseconds ?? 0,
            context,
            result.IsSuccess);
        return Task.CompletedTask;
    }
}
