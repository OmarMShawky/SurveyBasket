using Microsoft.IdentityModel.Tokens;

namespace SurveyBasket.Contracts.Polls;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Length(3, 100)
            .WithMessage("{PropertyName} must have between {MinLength} and {MaxLength} characters");

        RuleFor(p => p.Summary)
            .NotEmpty()
            .Length(3, 1500)
            .WithMessage("{PropertyName} must have between {MinLength} and {MaxLength} characters");

        RuleFor(p => p.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(p => p)
            .Must(HasValidDates)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} must be greater than or equal to StartsAt");

        RuleFor(p => p.EndsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(p => p.StartsAt);

    }
    private static bool HasValidDates(PollRequest pollRequest)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return pollRequest.StartsAt >= today && pollRequest.EndsAt >= pollRequest.StartsAt;

    }
}