using Microsoft.EntityFrameworkCore;
using Mottu.Infrastructure.DbContext.Models;

public class AppDbContext : DbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Deliverer> Deliverers { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<BrandNewMotorcycle> BrandNewMotorcycles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MotorcycleEntity(modelBuilder);
        DelivererEntity(modelBuilder);
        RentalsEntity(modelBuilder);
        BrandNewMotorcycleEntity(modelBuilder);
    }

    private static void BrandNewMotorcycleEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandNewMotorcycle>(entity =>
        {
            entity.HasKey(m => m.Identificador);
            entity.Property(m => m.Ano).IsRequired();
            entity.Property(m => m.Modelo).IsRequired();
            entity.Property(m => m.Placa).IsRequired();

            entity.HasIndex(m => m.Identificador)
                .IsUnique();

            entity.HasIndex(m => m.Placa)
                .IsUnique();
        });
    }

    private static void RentalsEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Plano", "\"Plano\" IN (7, 15, 30, 45, 50)"));
            entity.HasKey(r => r.Identificador);
            entity.Property(r => r.EntregadorId).IsRequired();
            entity.Property(r => r.MotoId).IsRequired();
            entity.Property(r => r.DataInicio).IsRequired();
            entity.Property(r => r.DataTermino).IsRequired();
            entity.Property(r => r.DataPrevisaoTermino).IsRequired();
            entity.Property(r => r.DataDevolucao);
            entity.Property(r => r.Plano).IsRequired();
        });

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Motorcycle)
            .WithMany(m => m.Rentals)
            .HasForeignKey(r => r.MotoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Deliverer)
            .WithMany(m => m.Rentals)
            .HasForeignKey(r => r.EntregadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void DelivererEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deliverer>(entity =>
        {
            entity.HasKey(d => d.Identificador);
            entity.Property(d => d.Nome).IsRequired().HasMaxLength(200);
            entity.Property(d => d.Cnpj).IsRequired().HasMaxLength(14);
            entity.Property(d => d.DataNascimento).IsRequired();
            entity.Property(d => d.NumeroCnh).IsRequired().HasMaxLength(20);
            entity.Property(d => d.TipoCnh).IsRequired().HasMaxLength(3);

            entity.HasIndex(m => m.Identificador)
                .IsUnique();

            entity.HasIndex(m => m.Cnpj)
                .IsUnique();

            entity.HasIndex(m => m.NumeroCnh)
                .IsUnique();
        });
    }

    private static void MotorcycleEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Motorcycle>(entity =>
        {
            entity.HasKey(m => m.Identificador);
            entity.Property(m => m.Ano).IsRequired();
            entity.Property(m => m.Modelo).IsRequired();
            entity.Property(m => m.Placa).IsRequired();

            entity.HasIndex(m => m.Identificador)
                .IsUnique();

            entity.HasIndex(m => m.Placa)
                .IsUnique();
        });
    }
}
