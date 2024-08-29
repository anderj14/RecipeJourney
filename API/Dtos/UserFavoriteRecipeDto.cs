
using Core.Entities;

namespace API.Dtos
{
    public class UserFavoriteRecipeDto
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int RecipeId { get; set; }
        // public string UserId { get; set; }
    }
}