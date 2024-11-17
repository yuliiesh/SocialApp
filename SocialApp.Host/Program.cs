using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("db")
    .WithDataVolume("mongo")
    .AddDatabase("SocialApp", "SocialApp");

var sql = builder.AddMySql("users-db")
    .WithDataVolume("users")
    .AddDatabase("users");

var neo4j = builder.AddDockerfile("neo4j", "../Neo4j")
    .WithEnvironment("NEO4J_AUTH", "neo4j/adminadmin")
    .WithVolume("neo4j", "/data", isReadOnly: false)
    .WithHttpEndpoint(port: 7687, targetPort: 7687);

builder.AddProject<SocialApp_Api>("socialapp-api")
    .WithReference(mongo)
    .WithReference(sql);

var dockerfile = builder
    .AddDockerfile("socialapp-media", "../", "./SocialApp.Media/Dockerfile")
    .WithVolume("images", "/app/images", isReadOnly: false)
    .WithHttpEndpoint(port: 5083, targetPort: 8080);

builder.AddProject<SocialApp_Client>("socialapp-client");

builder.Build().Run();