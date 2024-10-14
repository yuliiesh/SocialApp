using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface IPostRepository : IRepositoryBase<PostModel>
{
}

public class PostRepository : RepositoryBase<PostModel>, IPostRepository
{
    public PostRepository(SocialDbContext dbContext)
        : base(dbContext, "posts") { }
}