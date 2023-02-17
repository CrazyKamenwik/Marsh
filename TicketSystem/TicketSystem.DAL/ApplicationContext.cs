using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Entities;

namespace TicketSystem.DAL;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<TicketEntity> Tickets { get; set; } = null!;
    public DbSet<MessageEntity> Messages { get; set; } = null!;
    public DbSet<UserRoleEntity> UserRole { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEntity>()
            .HasOne(t => t.TicketCreator)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.TicketCreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketEntity>()
            .HasOne(t => t.Operator)
            .WithMany()
            .HasForeignKey(t => t.OperatorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MessageEntity>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MessageEntity>()
            .HasOne(m => m.Ticket)
            .WithMany(t => t.Messages)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}