
using System.Reflection;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class RecipeJourneyContext : IdentityDbContext<AppUser>
    {
        public RecipeJourneyContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Category> Categories { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }
        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<Instruction> Instructions { get; set; }
        DbSet<Recipe> Recipes { get; set; }
        DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeJourneyContext).Assembly);
        }
    }
}