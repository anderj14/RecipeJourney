using Core.Entities;

namespace Core.Specification
{
    public class CommentSpecification : BaseSpecification<Comment>
    {
        public CommentSpecification()
        {
            AddInclude(c => c.AppUser);
        }
        
        public CommentSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(c => c.AppUser);
        }

        public CommentSpecification(int recipeId, bool getByRecipeId = false)
        : base(x => getByRecipeId ? x.RecipeId == recipeId : x.RecipeId == recipeId)
        {
            AddInclude(c => c.AppUser);
        }
    }
}