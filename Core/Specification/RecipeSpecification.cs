
using Core.Entities;

namespace Core.Specification
{
    public class RecipeSpecification : BaseSpecification<Recipe>
    {
        public RecipeSpecification(RecipeSpecParams recipeSpecParams)
            : base(x =>
                (string.IsNullOrEmpty(recipeSpecParams.Search) || x.Title.ToLower()
                .Contains(recipeSpecParams.Search.ToLower())) &&
                (!recipeSpecParams.CategoryId.HasValue || x.CategoryId == recipeSpecParams.CategoryId)
            )
        {
            AddCommonIncludes();
            ApplySorting(recipeSpecParams.Sort);

            // Add Paging support
            ApplyPaging(recipeSpecParams.PageSize * (recipeSpecParams.PageIndex - 1),
                        recipeSpecParams.PageSize);
        }

        public RecipeSpecification(int id)
            : base(r => r.Id == id)
        {
            AddCommonIncludes();
        }

        private void AddCommonIncludes()
        {
            AddInclude(r => r.Category);
        }

        private void ApplySorting(string sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "preparationTimeAsc":
                        AddOrderBy(r => r.PreparationTime);
                        break;
                    case "preparationTimeDesc":
                        AddOrderByDescending(r => r.PreparationTime);
                        break;
                    case "cookingTimeAsc":
                        AddOrderBy(r => r.CookingTime);
                        break;
                    case "cookingTimeDesc":
                        AddOrderByDescending(r => r.CookingTime);
                        break;
                    default:
                        AddOrderBy(r => r.Title);
                        break;
                }
            }
            else
            {
                AddOrderBy(r => r.Title);
            }
        }
    }

}