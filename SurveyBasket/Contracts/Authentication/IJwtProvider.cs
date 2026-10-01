namespace SurveyBasket.Contracts.Authentication;

public interface IJwtProvider
{
    (string token, int duration) GenerateToken(ApplicationUser user, CancellationToken cancellationToken);
    string? ValidateToken(string token);
}