
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateInstructionDto
    {
        [Required]
        public int StepNumber { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int RecipeId { get; set; }
    }
}