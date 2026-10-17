namespace SurveyBasket.Contracts.Results;

public record VotesPerDayResponse(
    DateOnly Day,
    int NumberOfVotes);
