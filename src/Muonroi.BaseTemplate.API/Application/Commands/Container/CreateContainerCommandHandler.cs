namespace Muonroi.BaseTemplate.API.Application.Commands.Container;

public sealed class CreateContainerCommandHandler(RuleOrchestrator<CreateContainerCommand> orchestrator)
    : IRequestHandler<CreateContainerCommand, MResponse<string>>
{
    private readonly RuleOrchestrator<CreateContainerCommand> _orchestrator = orchestrator;

    public async Task<MResponse<string>> Handle(CreateContainerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _orchestrator.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
            return new MResponse<string> { Result = request.Code };
        }
        catch (InvalidOperationException ex)
        {
            MResponse<string> response = new();
            response.AddErrorMessage("ContainerError", ex.Message);
            return response;
        }
    }
}
