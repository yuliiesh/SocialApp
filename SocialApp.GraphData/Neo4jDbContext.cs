using Neo4jClient;

namespace SocialApp.GraphData;

public class Neo4jDbContext
{
    public Neo4jDbContext(string uri, string username, string password)
    {
        Client = new GraphClient(new Uri(uri), username, password);
        Client.ConnectAsync().Wait();
    }

    public IGraphClient Client { get; }
}