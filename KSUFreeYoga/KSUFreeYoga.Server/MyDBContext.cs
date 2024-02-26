using Microsoft.EntityFrameworkCore;

namespace KSUFreeYoga.Server
{
    public class MyDBContext : DbContext
    {

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }

        // Example of DbSet properties
        public DbSet<Location> Locations { get; set; }
        public DbSet<YogaClass> YogaClasses { get; set; }
    }
}
