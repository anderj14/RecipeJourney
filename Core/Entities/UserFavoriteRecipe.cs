
using Core.Entities.Identity;

namespace Core.Entities
{
    public class UserFavoriteRecipe : BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}