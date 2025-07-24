namespace Muonroi.BaseTemplate.API.Controllers
{
    public class AuthController(
    IMediator mediator,
    IAuthService<Permission, BaseTemplateDbContext> authService,
    IPermissionService<Permission, BaseTemplateDbContext> permissionService)
    : MAuthControllerBase<Permission, BaseTemplateDbContext>(authService, permissionService)
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            MResponse<LoginResponseModel> result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            MResponse<RefreshTokenResponseModel> result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result.GetActionResult();
        }
    }

}
