namespace SurveyBasket.Persistence.EntityConfigurations;

public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(va => new { va.VoteId, va.QuestionId }).IsUnique();
    }
}
