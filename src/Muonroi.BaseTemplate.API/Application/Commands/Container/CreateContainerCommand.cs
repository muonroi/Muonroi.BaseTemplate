namespace Muonroi.BaseTemplate.API.Application.Commands.Container;

public sealed record CreateContainerCommand(string Code) : IRequest<MResponse<string>>;
