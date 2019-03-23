using System.Data.Common;
using System.Data.Entity;

namespace Amazonspider.Core.EntityFramework
{
    public class AmazonspiderDbContext : DbContext
    {

        public static object sqlLiteLock = new object();
        public virtual DbSet<TaskSchedule> TaskSchedules { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }


        public AmazonspiderDbContext() : base("SqliteDefault")
        {

        }

        public AmazonspiderDbContext(DbConnection dbContext) : base(dbContext,true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskSchedule>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<ProductImage>();
        }
    }
}
