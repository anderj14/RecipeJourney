

using Core.Entities;

namespace Core.Specification
{
    public class RecipeFilterForCountSpecification : BaseSpecification<Recipe>
    {
        public RecipeFilterForCountSpecification(RecipeSpecParams recipeSpecParams)
            : base(x =>
                (string.IsNullOrEmpty(recipeSpecParams.Search) || x.Title.ToLower()
                .Contains(recipeSpecParams.Search.ToLower())) &&
                (!recipeSpecParams.CategoryId.HasValue || x.CategoryId == recipeSpecParams.CategoryId)
            )
        {
            // Ensure no pagination is applied here
        }
    }

}