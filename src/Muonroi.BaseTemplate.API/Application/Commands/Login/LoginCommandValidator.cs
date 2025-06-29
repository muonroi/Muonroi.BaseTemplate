namespace Muonroi.BaseTemplate.API.Application.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            _ = RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters");

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters");
        }
    }
}
