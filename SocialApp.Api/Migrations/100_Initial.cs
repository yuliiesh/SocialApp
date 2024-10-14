using MongoDB.Driver;
using MongoDBMigrations;
using Version = MongoDBMigrations.Version;

namespace SocialApp.Api.Migrations;

public class InitialMigration : IMigration
{
    public Version Version => new(1, 0, 0);

    public string Name => nameof(InitialMigration);

    public void Up(IMongoDatabase database)
    {
        database.CreateCollection("posts");
        database.CreateCollection("comments");
        database.CreateCollection("reactions");
    }

    public void Down(IMongoDatabase database)
    {
    }
}