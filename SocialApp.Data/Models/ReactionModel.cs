namespace SocialApp.Data.Models;

public class ReactionModel : ModelBase
{
    public string Type { get; set; }
    public Guid? PostId { get; set; }
    public Guid? CommentId { get; set; }
}