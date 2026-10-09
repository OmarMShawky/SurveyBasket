namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasIndex(q => new { q.Id, q.PollId })
               .IsUnique();

        builder.Property(q => q.Content)
               .HasMaxLength(500);
    }
}
