namespace SurveyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote = new
        ("Vote.DuplicatedVote", "User has already voted for this poll.", StatusCodes.Status409Conflict);

    public static readonly Error InvalidQuestions = new
        ("Vote.InvalidQuestions", "The questions provided in the request are invalid", StatusCodes.Status400BadRequest);
    
    public static readonly Error InvalidAnswers = new
        ("Vote.InvalidAnswers", "The answers provided in the question are invalid", StatusCodes.Status400BadRequest);
}