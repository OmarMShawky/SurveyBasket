namespace SurveyBasket.Entities;

public sealed class Answer
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public int QuestionId { get; set; }
    public bool IsActive { get; set; } = true;

    public Question Question { get; set; } = null!;
}
