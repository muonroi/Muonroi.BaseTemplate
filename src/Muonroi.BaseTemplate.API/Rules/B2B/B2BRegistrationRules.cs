using Muonroi.RuleEngine.Abstractions;

namespace Muonroi.BaseTemplate.API.Rules.B2B;

public record B2BRegistrationContext(string TaxCode, string DeclaredName, string DeclaredIndustry);

public record CompanyInfo(string TaxCode, string Name, string IndustryCode);

public interface ITaxAuthorityClient
{
    Task<bool> TaxCodeExistsAsync(string taxCode, CancellationToken cancellationToken = default);
    Task<CompanyInfo?> GetCompanyInfoAsync(string taxCode, CancellationToken cancellationToken = default);
}

public interface IFraudCheckClient
{
    Task<bool> IsBlacklistedAsync(string taxCode, CancellationToken cancellationToken = default);
}

public interface IIndustryClassifier
{
    Task<bool> IsRestrictedAsync(string industryCode, CancellationToken cancellationToken = default);
}

[RuleGroup("b2b-registration")]
public sealed class TaxCodeExistsRule : IRule<B2BRegistrationContext>
{
    private readonly ITaxAuthorityClient _client;
    public TaxCodeExistsRule(ITaxAuthorityClient client) => _client = client;
    public string Name => "TaxCodeExists";
    public IEnumerable<Type> Dependencies => Array.Empty<Type>();

    public string Code => nameof(TaxCodeExistsRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => Array.Empty<string>();
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(B2BRegistrationContext context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async Task<RuleResult> EvaluateAsync(B2BRegistrationContext context, FactBag facts, CancellationToken cancellationToken = default)
    {
        bool exists = await _client.TaxCodeExistsAsync(context.TaxCode, cancellationToken);
        facts["tax_code_exists"] = exists;
        return exists ? RuleResult.Passed() : RuleResult.Failure("Tax code not found");
    }
}

[RuleGroup("b2b-registration")]
public sealed class CompanyInfoMatchRule : IRule<B2BRegistrationContext>
{
    private readonly ITaxAuthorityClient _client;
    public CompanyInfoMatchRule(ITaxAuthorityClient client) => _client = client;
    public string Name => "CompanyInfoMatch";
    public IEnumerable<Type> Dependencies => new[] { typeof(TaxCodeExistsRule) };

    public string Code => nameof(CompanyInfoMatchRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => new[] { nameof(TaxCodeExistsRule) };
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(B2BRegistrationContext context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async Task<RuleResult> EvaluateAsync(B2BRegistrationContext context, FactBag facts, CancellationToken cancellationToken = default)
    {
        CompanyInfo? info = await _client.GetCompanyInfoAsync(context.TaxCode, cancellationToken);
        if (info is null || !string.Equals(info.Name, context.DeclaredName, StringComparison.OrdinalIgnoreCase))
        {
            return RuleResult.Failure("Declared data does not match official records");
        }
        facts["company_info"] = info;
        return RuleResult.Passed();
    }
}

[RuleGroup("b2b-registration")]
public sealed class BlacklistRule : IRule<B2BRegistrationContext>
{
    private readonly IFraudCheckClient _client;
    public BlacklistRule(IFraudCheckClient client) => _client = client;
    public string Name => "Blacklist";
    public IEnumerable<Type> Dependencies => new[] { typeof(CompanyInfoMatchRule) };

    public string Code => nameof(BlacklistRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => new[] { nameof(CompanyInfoMatchRule) };
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(B2BRegistrationContext context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async Task<RuleResult> EvaluateAsync(B2BRegistrationContext context, FactBag facts, CancellationToken cancellationToken = default)
    {
        if (!facts.TryGet<CompanyInfo>("company_info", out var info))
        {
            return RuleResult.Failure("Missing company info");
        }
        bool blacklisted = await _client.IsBlacklistedAsync(info.TaxCode, cancellationToken);
        return blacklisted ? RuleResult.Failure("Company is blacklisted") : RuleResult.Passed();
    }
}

[RuleGroup("b2b-registration")]
public sealed class IndustryRestrictionRule : IRule<B2BRegistrationContext>
{
    private readonly IIndustryClassifier _client;
    public IndustryRestrictionRule(IIndustryClassifier client) => _client = client;
    public string Name => "IndustryRestriction";
    public IEnumerable<Type> Dependencies => new[] { typeof(CompanyInfoMatchRule) };

    public string Code => nameof(IndustryRestrictionRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => new[] { nameof(CompanyInfoMatchRule) };
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(B2BRegistrationContext context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async Task<RuleResult> EvaluateAsync(B2BRegistrationContext context, FactBag facts, CancellationToken cancellationToken = default)
    {
        if (!facts.TryGet<CompanyInfo>("company_info", out var info))
        {
            return RuleResult.Failure("Missing company info");
        }
        bool restricted = await _client.IsRestrictedAsync(info.IndustryCode, cancellationToken);
        return restricted ? RuleResult.Failure("Industry is restricted") : RuleResult.Passed();
    }
}

