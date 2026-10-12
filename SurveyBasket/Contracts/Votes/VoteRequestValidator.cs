namespace SurveyBasket.Contracts.Votes;

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(v => v.Answers)
            .NotEmpty().WithMessage("Answers cannot be empty.")
            .ForEach(answer =>
            {
                answer.SetValidator(new VoteAnswerRequestValidator());
            });
    }
}
