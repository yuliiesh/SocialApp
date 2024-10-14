using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface ICommentRepository : IRepositoryBase<CommentModel>
{
}

public class CommentRepository : RepositoryBase<CommentModel>, ICommentRepository
{
    public CommentRepository(SocialDbContext dbContext)
        : base(dbContext, "comments") { }
}