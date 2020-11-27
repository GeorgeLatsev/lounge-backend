using Lounge.Services.Users.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lounge.Services.Users.Infrastructure.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }

    public class UsersContextDesignFactory : IDesignTimeDbContextFactory<UsersContext>
    {
        public UsersContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersContext>()
                .UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog=Lounge.Services.UsersDb;Integrated Security=true");

            return new UsersContext(optionsBuilder.Options);
        }
    }
}
