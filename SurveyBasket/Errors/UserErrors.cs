namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    public static readonly Error UserAlreadyExists =
        new("UserErrors.UserAlreadyExists", "User with same email already exists", StatusCodes.Status409Conflict);
}

//public static readonly Error InvalidRefreshToken =
//    new("InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

//public static readonly Error RefreshTokenExpired =
//    new("RefreshTokenExpired", "Refresh token has expired", StatusCodes.Status401Unauthorized);

//public static readonly Error RefreshTokenNotFound =
//    new("RefreshTokenNotFound", "Refresh token not found", StatusCodes.Status404NotFound);

//public static readonly Error UserNotFound =
//    new("UserNotFound", "User not found", StatusCodes.Status404NotFound);

//public static readonly Error UserAlreadyExists =
//    new("UserAlreadyExists", "User with the same email already exists", StatusCodes.Status409Conflict);

//public static readonly Error InvalidToken =
//    new("InvalidToken", "Invalid token", StatusCodes.Status401Unauthorized);
