using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data;

public class SocialDbContext
{
    private readonly IMongoDatabase _database;

    public SocialDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    // Generic method to get a collection of any type that inherits from ModelBase
    public IMongoCollection<T> GetCollection<T>(string collectionName) where T : ModelBase
    {
        return _database.GetCollection<T>(collectionName);
    }
}