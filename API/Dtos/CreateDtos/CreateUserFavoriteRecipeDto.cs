
namespace API.Dtos.CreateDtos
{
    public class CreateUserFavoriteRecipeDto
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int RecipeId { get; set; }
    }
}