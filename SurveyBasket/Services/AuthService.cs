namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return null;

        var (token, duration) = _jwtProvider.GenerateToken(user, cancellationToken);
        
        return new AuthResponse(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName!,
            LastName: user.LastName!,
            Token: token,
            ExpiresIn: duration
        );
    }
}