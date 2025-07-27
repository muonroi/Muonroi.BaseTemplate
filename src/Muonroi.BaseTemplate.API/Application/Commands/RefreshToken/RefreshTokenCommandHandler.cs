namespace Muonroi.BaseTemplate.API.Application.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(
        MAuthenticateInfoContext tokenInfo,
        IAuthenticateRepository authenticateRepository,
        Serilog.ILogger logger,
        IMJsonSerializeService jsonSerializeService,
        IMDateTimeService mDateTimeService) : MBaseHandler(logger, tokenInfo, jsonSerializeService, mDateTimeService), IRequestHandler<RefreshTokenCommand, MResponse<RefreshTokenResponseModel>>
    {
        public async Task<MResponse<RefreshTokenResponseModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            MResponse<RefreshTokenResponseModel> result = new();

            if (string.IsNullOrEmpty(AuthContext?.AccessToken))
            {
                result.AddErrorMessage("InvalidCredentials");
                return result;
            }

            MResponse<string> tokenIsValid = await authenticateRepository.ValidateTokenValidity(AuthContext.TokenValidityKey, cancellationToken);

            if (!tokenIsValid.IsOK || string.IsNullOrEmpty(tokenIsValid.Result))
            {
                result.AddErrorMessage("InvalidCredentials");
                return result;
            }

            MResponse<RefreshTokenResponseModel> newToken = await authenticateRepository.RefreshToken(
                new RefreshTokenRequestModel { AccessToken = AuthContext.AccessToken, RefreshToken = tokenIsValid.Result }, cancellationToken);

            if (!newToken.IsOK)
            {
                result.AddErrorMessage("InvalidCredentials");
                return result;
            }

            result.Result = newToken.Result;
            return result;
        }
    }

}
