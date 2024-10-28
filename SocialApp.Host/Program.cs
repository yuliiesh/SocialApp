using k8s.Models;
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

var dockerfile = builder
    .AddDockerfile("socialapp-media", "../", "./SocialApp.Media/Dockerfile")
    .WithVolume("images", "/app/images", isReadOnly: false)
    .WithHttpEndpoint(port: 5083, targetPort: 8080);

builder.AddProject<SocialApp_Client>("socialapp-client");

builder.Build().Run();