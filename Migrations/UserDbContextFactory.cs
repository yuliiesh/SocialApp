using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SocialApp.Data
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

            optionsBuilder.UseMySQL("Server=localhost;Port=54226;User ID=root;Password=!1G!784mn0quKnermF!KXp;Database=users");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}