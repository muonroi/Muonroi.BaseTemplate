

namespace Muonroi.BaseTemplate.API.Application.Commands.Rules;

public sealed record EvaluateNumberQuery(int Value) : IRequest<MResponse<FactBag>>;
