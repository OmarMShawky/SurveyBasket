namespace SurveyBasket.Contracts.Authentication;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(r => r.Token).NotEmpty();
        RuleFor(rt => rt.RefreshToken).NotEmpty();
    }
}
