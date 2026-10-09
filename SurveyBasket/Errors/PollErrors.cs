namespace SurveyBasket.Errors;

public static class PollErrors
{
    public static readonly Error NotFound
        = new("Poll.NotFound", "No poll found with the given Id", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedTitle
        = new("Poll.DuplicatedTitle", "Another poll with the same title already exists", StatusCodes.Status409Conflict);
}
