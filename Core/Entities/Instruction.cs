
namespace Core.Entities
{
    public class Instruction : BaseEntity
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }
        
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}