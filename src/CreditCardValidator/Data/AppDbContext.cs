using CreditCardValidator.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditCardValidator.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Card> Cards => Set<Card>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("Cards");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CardholderName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.BirthDate)
                .IsRequired();

            entity.Property(x => x.Brand)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.LastFourDigits)
                .IsRequired()
                .HasMaxLength(4);

            entity.Property(x => x.CreatedAt)
                .IsRequired();
        });
    }
}