
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int Rating { get; set; } // 1 to 5
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int RecipeId { get; set; }
    }
}