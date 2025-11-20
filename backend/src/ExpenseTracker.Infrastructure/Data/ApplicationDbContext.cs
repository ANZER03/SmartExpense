using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Receipt> Receipts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Categories)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Receipt)
            .WithOne(r => r.Expense)
            .HasForeignKey<Receipt>(r => r.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasColumnType("decimal(18,2)");
            
        modelBuilder.Entity<Receipt>()
            .Property(r => r.TotalAmount)
            .HasColumnType("decimal(18,2)");
    }
}
