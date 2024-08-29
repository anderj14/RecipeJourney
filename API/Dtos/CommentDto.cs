
namespace API.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; }
    }
}