using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Domain.Entities;


namespace TicketManagementSystem.Infrastructure.Data
{
    public class TicketManagementDbContext : DbContext
    {
        public TicketManagementDbContext(DbContextOptions<TicketManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
}