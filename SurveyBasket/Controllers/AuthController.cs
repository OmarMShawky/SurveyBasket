using Microsoft.Extensions.Options;

namespace SurveyBasket.Controllers;

[Route("auth")]
[ApiController]
public class AuthController(IAuthService authService, IOptions<JwtOptions> jwtOption) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly JwtOptions _jwtOption = jwtOption.Value;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return (authResult is null) ? BadRequest("Invalid Email or Password") : Ok(authResult);
    }
}