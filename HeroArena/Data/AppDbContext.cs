using Microsoft.EntityFrameworkCore;
using HeroArena.Models;
using HeroArena.Services;

namespace HeroArena.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Login> Logins { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<PlayerHero> PlayerHeroes { get; set; }
        public DbSet<HeroSpell> HeroSpells { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // ✅ Connection string dynamique depuis AppSettings
            options.UseSqlServer(AppSettings.Instance.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>().ToTable("Login");
            modelBuilder.Entity<Hero>().ToTable("Hero");
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Spell>().ToTable("Spell");
            modelBuilder.Entity<PlayerHero>().ToTable("PlayerHero");
            modelBuilder.Entity<HeroSpell>().ToTable("HeroSpell");

            modelBuilder.Entity<PlayerHero>()
                .HasKey(ph => new { ph.PlayerID, ph.HeroID });

            modelBuilder.Entity<HeroSpell>()
                .HasKey(hs => new { hs.HeroID, hs.SpellID });
        }
    }
}