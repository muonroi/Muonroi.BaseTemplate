


namespace Muonroi.BaseTemplate.API.Application.Commands.Login
{
    public class LoginCommandHandler(IAuthenticateRepository authenticateRepository) : IRequestHandler<LoginCommand, MResponse<LoginResponseModel>>
    {
        private readonly IAuthenticateRepository _authenticateRepository = authenticateRepository;

        public async Task<MResponse<LoginResponseModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            MResponse<LoginResponseModel> result = new();
            MResponse<LoginResponseModel> authResult = await _authenticateRepository.Login(
                new LoginRequestModel { Username = request.Username, Password = request.Password }, cancellationToken);

            if (!authResult.IsOK)
            {
                result.AddErrors(authResult.ErrorMessages);
                return result;
            }
            return authResult;
        }
    }

}
