using Muonroi.RuleEngine.Abstractions;

namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("numbers")]
public sealed class EvenRule : IRule<int>
{
    public string Name => "Even";
    public IEnumerable<Type> Dependencies => [typeof(PositiveRule)];

    public string Code => nameof(EvenRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => [nameof(PositiveRule)];
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(int context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<RuleResult> EvaluateAsync(int context, FactBag facts, CancellationToken cancellationToken = default)
    {
        bool result = context % 2 == 0;
        facts["even"] = result;
        return Task.FromResult(result ? RuleResult.Passed() : RuleResult.Failure("Number must be even"));
    }
}
