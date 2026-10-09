namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error NotFound =
        new("Question.NotFound", "No question found with the given Id", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedContent =
        new("Question.DuplicatedContent", "A question with the same content already exists", StatusCodes.Status409Conflict);
}
