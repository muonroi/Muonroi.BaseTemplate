namespace Muonroi.BaseTemplate.API.Application.Commands.Rules;

public sealed class EvaluateNumberQueryHandler(RuleOrchestrator<int> orchestrator)
    : IRequestHandler<EvaluateNumberQuery, MResponse<FactBag>>
{
    private readonly RuleOrchestrator<int> _orchestrator = orchestrator;

    public async Task<MResponse<FactBag>> Handle(EvaluateNumberQuery request, CancellationToken cancellationToken)
    {
        FactBag facts = await _orchestrator.ExecuteAsync(request.Value, cancellationToken).ConfigureAwait(false);
        return new MResponse<FactBag> { Result = facts };
    }
}
