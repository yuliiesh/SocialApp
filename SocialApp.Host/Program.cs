using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("db")
    .WithDataVolume("mongo")
    .AddDatabase("SocialApp", "SocialApp");

var sql = builder.AddMySql("users-db")
    .WithDataVolume("users")
    .AddDatabase("users");

builder.AddProject<SocialApp_Api>("socialapp-api")
    .WithReference(mongo)
    .WithReference(sql);

builder.AddProject<SocialApp_Client>("socialapp-client");

builder.Build().Run();