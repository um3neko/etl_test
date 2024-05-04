using ETL_test.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace ETL_test.DbContexts
{
    public class ObjectModelDbContext(DbContextOptions<ObjectModelDbContext> options) : DbContext(options)
    {
        public DbSet<ObjectModel> ObjectModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
