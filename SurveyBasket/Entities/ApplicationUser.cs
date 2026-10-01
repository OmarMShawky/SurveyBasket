namespace SurveyBasket.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}
