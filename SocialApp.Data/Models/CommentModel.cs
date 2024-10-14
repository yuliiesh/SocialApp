namespace SocialApp.Data.Models;

public class CommentModel : ModelBase
{
    public string Content { get; set; }
    public Guid PostId { get; set; }
}