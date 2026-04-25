using Microsoft.EntityFrameworkCore;
using HeroArena.Models;

namespace HeroArena.Data
{
    public class AppDbContext : DbContext
    {
        // ===== TABLES =====
        public DbSet<Login> Logins { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<PlayerHero> PlayerHeroes { get; set; }
        public DbSet<HeroSpell> HeroSpells { get; set; }

        // ===== CONFIG DB =====
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                "Server=localhost\\SQLEXPRESS01;" +
                "Database=ExerciceHero;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;"
            );
        }

        // ===== CONFIG TABLES / RELATIONS =====
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ⚠️ FIX NOMS (IMPORTANT)
            modelBuilder.Entity<Login>().ToTable("Login");
            modelBuilder.Entity<Hero>().ToTable("Hero");
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Spell>().ToTable("Spell");
            modelBuilder.Entity<PlayerHero>().ToTable("PlayerHero");
            modelBuilder.Entity<HeroSpell>().ToTable("HeroSpell");

            // ===== RELATIONS MANY-TO-MANY =====
            modelBuilder.Entity<PlayerHero>()
                .HasKey(ph => new { ph.PlayerID, ph.HeroID });

            modelBuilder.Entity<HeroSpell>()
                .HasKey(hs => new { hs.HeroID, hs.SpellID });
        }
    }
}