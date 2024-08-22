
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class UserFavoriteRecipeConfig : IEntityTypeConfiguration<UserFavoriteRecipe>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteRecipe> builder)
        {
            builder.HasKey(ufr => new { ufr.UserId, ufr.RecipeId });

            builder.HasOne(ufr => ufr.AppUser)
            .WithMany(u => u.UserFavoriteRecipes)
            .HasForeignKey(ufr => ufr.UserId);

            builder.HasOne(ufr => ufr.Recipe)
            .WithMany(r => r.UserFavoriteRecipes)
            .HasForeignKey(ufr => ufr.RecipeId);
        }
    }
}