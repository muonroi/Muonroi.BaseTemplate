namespace Muonroi.BaseTemplate.API.Application.Commands.Sample
{
    public class GetSamplesQueryHandler(ISampleQuery query)
        : IRequestHandler<GetSamplesQuery, MResponse<IEnumerable<SampleDto>>>
    {
        private readonly ISampleQuery _query = query;

        public async Task<MResponse<IEnumerable<SampleDto>>> Handle(GetSamplesQuery request, CancellationToken cancellationToken)
        {
            List<SampleEntity>? entities = await _query.GetAllAsync().ConfigureAwait(false);
            IEnumerable<SampleDto> dtos = entities?.Select(e => new SampleDto { Id = e.Id, Name = e.Name }) ?? [];
            return new MResponse<IEnumerable<SampleDto>> { Result = dtos };
        }
    }
}
