namespace SurveyBasket.Entities;

public sealed class Poll : AuditableEntity
{
    public int Id { get; init; }
    public string Title { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public bool IsPublished { get; set; }
    public DateOnly StartsAt { get; set; }
    public DateOnly EndsAt { get; set; }

    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<Vote> Votes { get; set; } = [];

}
