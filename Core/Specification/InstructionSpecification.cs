
using Core.Entities;

namespace Core.Specification
{
    public class InstructionSpecification : BaseSpecification<Instruction>
    {
        public InstructionSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(i => i.Recipe);
        }

        public InstructionSpecification(int recipeId, bool getByRecipeId = false)
        : base(x => getByRecipeId ? x.RecipeId == recipeId : x.RecipeId == recipeId)
        {
            AddInclude(i => i.Recipe);
        }
    }
}