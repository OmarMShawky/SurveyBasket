namespace SurveyBasket.Contracts.Questions;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(q => q.Content)
            .NotEmpty()
            .Length(3, 500)
            .WithMessage("Question must be between 3 and 500 characters");

        RuleFor(q => q.Answers)
            .NotNull();

        RuleFor(q => q.Answers)
            .Must(q => q.Count > 1)
            .WithMessage("Question should have at least 2 answers")
            .When(q => q.Answers != null);

        RuleFor(q => q.Answers)
            .Must(q => q.Distinct().Count() == q.Count)
            .WithMessage("You cannot add duplicated answers for the question")
            .When(q => q.Answers != null);
    }
}
