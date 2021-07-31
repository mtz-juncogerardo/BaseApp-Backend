using BaseApp.Data.DbModels;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DataAccess
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<AuthenticationDbModel> Authentication { get; set; }
        public DbSet<AuditDbModel> Audit { get; set; }
    }
}