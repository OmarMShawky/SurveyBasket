using System.Security.Cryptography;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, duration) = _jwtProvider.GenerateToken(user, cancellationToken);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiredAt = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse
        (
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName!,
            LastName: user.LastName!,
            Token: token,
            ExpiresIn: duration,
            RefreshToken: refreshToken,
            RefreshTokenExpiration: refreshTokenExpiration
        );

        return Result.Success(response);
    }


    public async Task<Result<AuthResponse>> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken && t.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        userRefreshToken.RevokedAt = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, cancellationToken);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiredAt = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName!,
            LastName: user.LastName!,
            Token: newToken,
            ExpiresIn: expiresIn,
            RefreshToken: newRefreshToken,
            RefreshTokenExpiration: refreshTokenExpiration);

        return Result.Success(response);

    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidCredentials);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidCredentials);

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken && t.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidCredentials);

        userRefreshToken.RevokedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);
        return Result.Success();

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}