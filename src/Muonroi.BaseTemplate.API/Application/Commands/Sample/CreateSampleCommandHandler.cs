namespace Muonroi.BaseTemplate.API.Application.Commands.Sample
{
    public class CreateSampleCommandHandler(ISampleRepository repository)
        : IRequestHandler<CreateSampleCommand, MResponse<SampleDto>>
    {
        private readonly ISampleRepository _repository = repository;

        public async Task<MResponse<SampleDto>> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
        {
            SampleEntity entity = new() { Name = request.Name };
            _repository.Add(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            SampleDto dto = new() { Id = entity.Id, Name = entity.Name };
            return new MResponse<SampleDto> { Result = dto };
        }
    }
}
