namespace SurveyBasket.Controllers;

[Route("auth")]
[ApiController]

public class AuthController(
    IAuthService authService,
    IOptions<JwtOptions> jwtOptions,
    IConfiguration config) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration _config = config;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : BadRequest(authResult.Error);
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var RefreshTokenresponse = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return (RefreshTokenresponse is null) ? BadRequest("Invalid Token") : Ok(RefreshTokenresponse);

    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked ? Ok() : BadRequest("Invalid Token");

    }
    [HttpPost("test")]
    public ActionResult Test()
    {
        var result = new
        {
            _jwtOptions.Audience,
            _jwtOptions.Issuer,
            _jwtOptions.ExpiryMinutes
        };

        var result2 = new
        {
            MyKey = _config["MyKey"]
        };
        return Ok(result2);
    }
}