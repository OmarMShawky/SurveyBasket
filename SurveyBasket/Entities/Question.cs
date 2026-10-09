namespace SurveyBasket.Entities;

public sealed class Question : AuditableEntity
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int PollId { get; set; }

    public bool IsActive { get; set; } = true;

    public Poll Poll { get; set; } = null!;

    public ICollection<Answer> Answers { get; set; } = [];
}
