namespace SurveyBasket.Controllers;

[Route("auth")]
[ApiController]
[AllowAnonymous]

public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("login-username")]
    public async Task<IActionResult> LoginByUsername([FromBody] string username, string password, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenForUsernameAsync(username, password, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }


    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }

}