namespace SurveyBasket.Contracts.Votes;

public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(x => x.AnswerId)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
    }
}
