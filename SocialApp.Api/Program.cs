using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDBMigrations;
using SocialApp.Api;
using SocialApp.Api.Migrations;
using SocialApp.Data;

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase()
    .AddRepositories();

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
    .AddUserAuthorization();

var app = builder.Build();

var provider = app.Services;

var connectionString = provider.GetService<IConfiguration>().GetConnectionString("DefaultConnection");

var migrationEngine = new MigrationEngine()
    .UseDatabase(connectionString, "SocialApp")
    .UseAssembly(typeof(InitialMigration).Assembly)
    .UseSchemeValidation(false);
migrationEngine.Run();


app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
