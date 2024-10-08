using Core.Entities;

namespace Core.Specification
{
    public class IngredientSpecification : BaseSpecification<Ingredient>
    {
        public IngredientSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(i => i.Recipe);
        }

        public IngredientSpecification(int recipeId, bool getByRecipeId = false)
        : base(x => getByRecipeId ? x.RecipeId == recipeId : x.RecipeId == recipeId)
        {
            AddInclude(i => i.Recipe);
        }
    }
}