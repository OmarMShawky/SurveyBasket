namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("InvalidCredentials", "Invalid email/password");

}
