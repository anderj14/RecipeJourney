
namespace Core.Entities
{
    public class Ingredient: BaseEntity
    {
        public string IngredientName { get; set; }
        public string Quantity { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}