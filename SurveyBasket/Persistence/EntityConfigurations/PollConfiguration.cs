namespace SurveyBasket.Persistence.EntityConfigurations;

public class PollConfiguration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasIndex(p => p.Title)
               .IsUnique();

        builder.Property(p => p.Title)
               .HasMaxLength(500);

        builder.Property(p => p.Summary)
               .HasMaxLength(500);
    }
}
