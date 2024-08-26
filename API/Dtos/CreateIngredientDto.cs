
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateIngredientDto
    {
        [Required]
        public string IngredientName { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public int RecipeId { get; set; }
    }
}