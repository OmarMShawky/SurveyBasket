namespace SurveyBasket.Entities;

public sealed class Vote
{
    public int Id { get; set; }
    public int PollId { get; set; }
    public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = string.Empty;
    public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];

    public Poll Poll { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
