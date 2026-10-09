namespace SurveyBasket.Contracts.Polls;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Length(3, 500)
            .WithMessage("{PropertyName} must have between {MinLength} and {MaxLength} characters");

        RuleFor(p => p.Summary)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Length(3, 500)
            .WithMessage("{PropertyName} must have between {MinLength} and {MaxLength} characters");

        RuleFor(p => p.StartsAt)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
            //.Must(BeTodayOrLater)
            .WithMessage("{PropertyName} must be today or a future date");

        RuleFor(p => p.EndsAt)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(p => p.StartsAt)
            .WithMessage("{PropertyName} must be on or after {ComparisonProperty}");
    }
}


//        RuleFor(p => p)
//            .Must(HasValidDates)
//            .WithName(nameof(PollRequest.EndsAt))
//            .WithMessage("{PropertyName} must be greater than or equal to StartsAt");

//        RuleFor(p => p.EndsAt)
//            .NotEmpty()
//            .GreaterThanOrEqualTo(p => p.StartsAt);

//    }
//    private static bool HasValidDates(PollRequest pollRequest)
//    {
//        var today = DateOnly.FromDateTime(DateTime.Today);

//        return pollRequest.StartsAt >= today && pollRequest.EndsAt >= pollRequest.StartsAt;

//    }