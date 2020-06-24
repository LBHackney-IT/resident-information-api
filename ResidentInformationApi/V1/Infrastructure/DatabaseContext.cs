using Microsoft.EntityFrameworkCore;

namespace ResidentInformationApi.V1.Infrastructure
{

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DatabaseEntity> DatabaseEntities { get; set; }
    }
}
