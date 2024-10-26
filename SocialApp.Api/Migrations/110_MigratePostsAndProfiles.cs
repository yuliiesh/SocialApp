using MongoDB.Driver;
using MongoDBMigrations;
using SocialApp.Data.Models;
using Version = MongoDBMigrations.Version;

namespace SocialApp.Api.Migrations;

public class MigratePostsAndProfiles : IMigration
{
    public Version Version => new(1, 1, 0);

    public string Name => nameof(MigratePostsAndProfiles);

    public void Up(IMongoDatabase database)
    {
        return;
        var posts = database.GetCollection<PostModel>("posts");
        var profiles = database.GetCollection<ProfileModel>("profiles");
    }

    public void Down(IMongoDatabase database)
    {
    }
}