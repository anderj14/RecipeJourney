
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public int Rating { get; set; } // 1 to 5
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}