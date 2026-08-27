namespace SurveyBasket.Contracts.Authentication;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(l => l.Email).NotEmpty().EmailAddress();
        RuleFor(l => l.Password).NotEmpty();
    }
}