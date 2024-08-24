using System.ComponentModel.DataAnnotations;

namespace API.Dtos.CreateDtos
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; }
    }
}