namespace Muonroi.BaseTemplate.API.Controllers
{
    [ApiVersion("1.0")]
    public class AuthController(
    IMediator mediator,
    IAuthService<Permission, BaseTemplateDbContext> authService,
    IPermissionService<Permission, BaseTemplateDbContext> permissionService)
    : MAuthControllerBase<Permission, BaseTemplateDbContext>(authService, permissionService)
    {
        public override async Task<IActionResult> Login(
            [FromBody] LoginRequestModel request,
            [FromServices] MTokenInfo tokenInfo,
            [FromServices] MAuthenticateTokenHelper<Permission> tokenHelper,
            [FromServices] IMultiLevelCacheService cacheService,
            CancellationToken cancellationToken)
        {
            // Delegate to mediator-based command while preserving base signature and route
            MResponse<LoginResponseModel> result = await mediator.Send(new LoginCommand
            {
                Username = request.Username,
                Password = request.Password
            }, cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }

        public override async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequestModel request,
            [FromServices] MTokenInfo tokenInfo,
            [FromServices] MAuthenticateTokenHelper<Permission> tokenHelper,
            [FromServices] IMultiLevelCacheService cacheService,
            CancellationToken cancellationToken)
        {
            // Use mediator command; handler reads tokens from context/validation as designed
            MResponse<RefreshTokenResponseModel> result = await mediator.Send(new RefreshTokenCommand(), cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }
    }

}
