using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DbOracleContext : DbContext
    {
        public DbOracleContext()
        {
        }

        public DbOracleContext(DbContextOptions<DbOracleContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("CONS_INFO_CREG");
            modelBuilder.ApplyAllConfigurations();
        }
    }
}

