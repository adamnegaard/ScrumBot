using Microsoft.EntityFrameworkCore;

namespace ScrumBot.Entities
{
    public class ScrumContext : DbContext, IScrumContext
    {
        public ScrumContext(DbContextOptions<ScrumContext> options)
            : base(options)
        {
        }

        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Reminder> Reminders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sprint>(entity =>
            {
                entity
                    .HasKey(s => s.Id);

                entity
                    .HasMany(s => s.Tasks)
                    .WithOne(t => t.Sprint)
                    .HasForeignKey(t => t.SprintId);
            });
            modelBuilder.Entity<Task>(entity =>
            {
                entity
                    .HasKey(t => t.Id);
            });
            modelBuilder.Entity<Reminder>(entity =>
            {
                entity
                    .HasKey(r => new {r.StartTime, r.GuildId, r.Summary});
            });

            //For serial keys
            modelBuilder.UseSerialColumns();
        }
    }
}