using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.DbConfigurations
{
    public class SiemplifyExmDBContext : DbContext
    {
        public SiemplifyExmDBContext(DbContextOptions<SiemplifyExmDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>().Property(x => x.Id).HasMaxLength(100);
            modelBuilder.Entity<TaskEntity>().Property(x => x.ConsumerID).HasMaxLength(100);
            modelBuilder.Entity<TaskEntity>().Property(x => x.TaskText).HasMaxLength(1000);
            modelBuilder.Entity<TaskEntity>().ToTable("Task");
        }

        public DbSet<TaskEntity> Tasks { get; set; }
    }
}
