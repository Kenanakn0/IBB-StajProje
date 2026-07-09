using BiletSatis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Persistence;

public class BiletSatisDbContext : DbContext
{
    public BiletSatisDbContext(DbContextOptions<BiletSatisDbContext> options)
        : base(options) { }

    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
    public DbSet<Salon> Salonlar => Set<Salon>();
    public DbSet<Bolum> Bolumler => Set<Bolum>();
    public DbSet<Etkinlik> Etkinlikler => Set<Etkinlik>();
    public DbSet<EtkinlikBolum> EtkinlikBolumleri => Set<EtkinlikBolum>();
    public DbSet<Koltuk> Koltuklar => Set<Koltuk>();
    public DbSet<Rezervasyon> Rezervasyonlar => Set<Rezervasyon>();
    public DbSet<Bilet> Biletler => Set<Bilet>();
    public DbSet<Indirim> Indirimler => Set<Indirim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Para alanları: decimal(18,2) ---
        modelBuilder.Entity<EtkinlikBolum>().Property(x => x.Fiyat).HasPrecision(18, 2);
        modelBuilder.Entity<Bilet>().Property(x => x.OdenenTutar).HasPrecision(18, 2);
        modelBuilder.Entity<Indirim>().Property(x => x.Yuzde).HasPrecision(5, 2);

        // --- E-posta benzersiz olsun ---
        modelBuilder.Entity<Kullanici>().HasIndex(x => x.Eposta).IsUnique();

        // --- Cascade path çakışmasını önlemek için bazı ilişkileri Restrict yap ---
        // (Bilet ve Rezervasyon aynı anda birden çok tabloya bağlı olduğu için SQL Server
        //  otomatik cascade delete'te "multiple cascade paths" hatası verir. Bunu engelliyoruz.)

        modelBuilder.Entity<EtkinlikBolum>()
            .HasOne(eb => eb.Etkinlik)
            .WithMany(e => e.EtkinlikBolumleri)
            .HasForeignKey(eb => eb.EtkinlikId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EtkinlikBolum>()
            .HasOne(eb => eb.Bolum)
            .WithMany(b => b.EtkinlikBolumleri)
            .HasForeignKey(eb => eb.BolumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Koltuk>()
            .HasOne(k => k.Bolum)
            .WithMany(b => b.Koltuklar)
            .HasForeignKey(k => k.BolumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bilet>()
            .HasOne(b => b.EtkinlikBolum)
            .WithMany(eb => eb.Biletler)
            .HasForeignKey(b => b.EtkinlikBolumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bilet>()
            .HasOne(b => b.Koltuk)
            .WithMany()
            .HasForeignKey(b => b.KoltukId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rezervasyon>()
            .HasOne(r => r.EtkinlikBolum)
            .WithMany(eb => eb.Rezervasyonlar)
            .HasForeignKey(r => r.EtkinlikBolumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rezervasyon>()
            .HasOne(r => r.Koltuk)
            .WithMany()
            .HasForeignKey(r => r.KoltukId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}