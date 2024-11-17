using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDBMigrations;
using SocialApp.Api;
using SocialApp.Api.Migrations;
using SocialApp.Common;
using SocialApp.Data;
using SocialApp.GraphData;

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase()
    .AddRepositories()
    .AddHandlers();

builder.Services.AddTransient(provider =>
{
    var configuration = provider.GetService<IConfiguration>();
    var uri = configuration["Neo4j:Uri"];
    var username = configuration["Neo4j:Username"];
    var password = configuration["Neo4j:Password"];
    return new Neo4jDbContext(uri, username, password);
});

builder.AddServiceDefaults();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddUserDatabase(builder.Configuration)
    .AddUserAuthorization(builder.Configuration);

var app = builder.Build();

var provider = app.Services;

var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("SocialApp");

var migrationEngine = new MigrationEngine()
    .UseDatabase(connectionString, "SocialApp")
    .UseAssembly(typeof(InitialMigration).Assembly)
    .UseSchemeValidation(false);
migrationEngine.Run();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
