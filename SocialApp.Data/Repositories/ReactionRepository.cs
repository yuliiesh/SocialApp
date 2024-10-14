using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface IReactionRepository : IRepositoryBase<ReactionModel>
{
}

public class ReactionRepository : RepositoryBase<ReactionModel>, IReactionRepository
{
    public ReactionRepository(SocialDbContext dbContext)
        : base(dbContext, "reactions") { }
}