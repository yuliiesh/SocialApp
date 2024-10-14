using Microsoft.Extensions.DependencyInjection;
using SocialApp.Data;

var serviceCollection = new ServiceCollection();

serviceCollection.AddUserDatabase("Server=localhost;port=3306;database=users;user id=admin;password=admin");

var provider = serviceCollection.BuildServiceProvider();