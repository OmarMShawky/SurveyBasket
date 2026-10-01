namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Property(au => au.FirstName)
            .HasMaxLength(100);

        builder
            .Property(au => au.LastName)
            .HasMaxLength(100);

        builder
            .OwnsMany(t => t.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");
            
    }
}
