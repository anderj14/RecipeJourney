

namespace API.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public string DifficultyLevel { get; set; }
        public string PictureUrl { get; set; }
        public string CategoryName { get; set; }
    }
}