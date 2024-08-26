
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.CreateDtos
{
    public class CreateRecipeDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int PreparationTime { get; set; }
        [Required]
        public int CookingTime { get; set; }
        [Required]
        public string DifficultyLevel { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}