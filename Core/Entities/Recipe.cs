
namespace Core.Entities
{
    public class Recipe : BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public string DifficultyLevel { get; set; }
        public string PictureUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }
    }
}