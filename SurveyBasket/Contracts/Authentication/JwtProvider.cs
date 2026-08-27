namespace SurveyBasket.Contracts.Authentication;

public class JwtProvider : IJwtProvider
{
    public (string token, int duration) GenerateToken(ApplicationUser user, CancellationToken cancellationToken)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName!),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("j3MvPVLH2UOIkKoURASCZ291eqhvu3Ge"));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        const int expiresIn = 30;

        var token = new JwtSecurityToken(
            issuer: "SurveyBasketApp",
            audience: "SurveyBasketApp users",
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiresIn),
            signingCredentials: signingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), duration: expiresIn * 60);
    }
}