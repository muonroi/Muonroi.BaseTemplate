namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("numbers")]
public sealed class SquareRule : IRule<int>
{
    public string Name => "Square";
    public IEnumerable<Type> Dependencies => [typeof(RangeRule), typeof(EvenRule)];

    public string Code => nameof(SquareRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => [nameof(RangeRule), nameof(EvenRule)];
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Business;

    public Task ExecuteAsync(int context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<RuleResult> EvaluateAsync(int context, FactBag facts, CancellationToken cancellationToken = default)
    {
        int square = context * context;
        facts["square"] = square;
        return Task.FromResult(RuleResult.Passed());
    }
}
