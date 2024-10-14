using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SocialApp.Data
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

            optionsBuilder.UseMySQL("Server=localhost;port=3306;database=users;user id=admin;password=admin");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}