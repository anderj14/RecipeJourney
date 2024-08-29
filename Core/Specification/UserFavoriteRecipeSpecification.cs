
using Core.Entities;

namespace Core.Specification
{
    public class UserFavoriteRecipeSpecification : BaseSpecification<UserFavoriteRecipe>
    {
        public UserFavoriteRecipeSpecification(string userId)
        : base(uf => uf.UserId == userId)
        {
            AddInclude(uf => uf.AppUser);
            AddInclude(uf => uf.Recipe);
        }
    }

    public class RecipeFavoriteUsersSpecification : BaseSpecification<UserFavoriteRecipe>
    {
        public RecipeFavoriteUsersSpecification(int recipeId)
             : base(uf => uf.RecipeId == recipeId)
        {
            AddInclude(uf => uf.AppUser);
        }
    }
}