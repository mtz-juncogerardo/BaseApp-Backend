using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BaseApp.Data.DataAccess
{
    // This is used by Entity Framework so it can Add Migrations in this Project instead of the API Project
    public class DbContextFactory : IDesignTimeDbContextFactory<AuthenticationContext>
    {
        private static readonly string ConnectionString = DbConnectionString.DbConn;

        public AuthenticationContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuthenticationContext>(); 
            builder.UseNpgsql(ConnectionString); 
            return new AuthenticationContext(builder.Options);
        }
    }
}