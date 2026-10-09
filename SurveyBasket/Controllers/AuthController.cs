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

        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();


        //return result.IsSuccess
        //    ? Ok(result.Value)
        //    : BadRequest(result.Error);
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess
           ? Ok(authResult.Value)
           : authResult.ToProblem();
        //return (RefreshTokenresponse is not null)
        //    ? Ok(RefreshTokenresponse)
        //    : BadRequest("Invalid Token");

    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok(result) : result.ToProblem();

    }
    //[HttpPost("test")]
    //public ActionResult Test()
    //{
    //    var result = new
    //    {
    //        _jwtOptions.Audience,
    //        _jwtOptions.Issuer,
    //        _jwtOptions.ExpiryMinutes
    //    };

    //    var result2 = new
    //    {
    //        MyKey = _config["MyKey"]
    //    };
    //    return Ok(result2);
    //}
}