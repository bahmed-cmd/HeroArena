using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HeroArena.Data;
using HeroArena.Models;
using HeroArena.Services;

namespace HeroArena.ViewModels
{
    public class SettingsVMX : INotifyPropertyChanged
    {
        // ✅ Requis par l'énoncé
        public bool DebugFlag { get; set; } = false;

        private string _connectionString;
        public string ConnectionString
        {
            get => _connectionString;
            set { _connectionString = value; OnPropertyChanged(nameof(ConnectionString)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand SeedCommand { get; }

        public SettingsVMX()
        {
            ConnectionString = AppSettings.Instance.ConnectionString;
            SaveCommand = new RelayCommand(Save);
            SeedCommand = new RelayCommand(SeedDatabase);
        }

        private void Save()
        {
            try
            {
                AppSettings.Instance.ConnectionString = ConnectionString;
                AppSettings.Save();
                MessageBox.Show("✅ Connexion sauvegardée !", "Succès");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Erreur : " + ex.Message);
            }
        }

        private void SeedDatabase()
        {
            try
            {
                using var db = new AppDbContext();
                db.Database.EnsureCreated();

                if (db.Heroes.Any())
                {
                    MessageBox.Show("⚠️ La base contient déjà des données.");
                    return;
                }

                // --- Spells ---
                var fireball    = new Spell { Name = "Fireball",      Damage = 40, Description = "Une boule de feu dévastatrice." };
                var frostbolt   = new Spell { Name = "Frostbolt",     Damage = 30, Description = "Ralentit et blesse l'ennemi." };
                var arcaneBlast = new Spell { Name = "Arcane Blast",  Damage = 50, Description = "Explosion magique pure." };
                var blink       = new Spell { Name = "Blink",         Damage = 10, Description = "Téléportation rapide + frappe." };

                var slash       = new Spell { Name = "Slash",         Damage = 35, Description = "Coup d'épée puissant." };
                var shieldBash  = new Spell { Name = "Shield Bash",   Damage = 25, Description = "Frappe avec le bouclier." };
                var warcry      = new Spell { Name = "War Cry",       Damage = 20, Description = "Cri de guerre qui renforce." };
                var execute     = new Spell { Name = "Execute",       Damage = 60, Description = "Coup fatal sur ennemi affaibli." };

                var shadowStrike = new Spell { Name = "Shadow Strike", Damage = 45, Description = "Frappe depuis l'ombre." };
                var poisonBlade  = new Spell { Name = "Poison Blade",  Damage = 20, Description = "Empoisonne l'adversaire." };
                var smokeScreen  = new Spell { Name = "Smoke Screen",  Damage = 15, Description = "Crée de la confusion." };
                var backstab     = new Spell { Name = "Backstab",      Damage = 55, Description = "Coup dans le dos dévastateur." };

                db.Spells.AddRange(fireball, frostbolt, arcaneBlast, blink,
                                   slash, shieldBash, warcry, execute,
                                   shadowStrike, poisonBlade, smokeScreen, backstab);

                // --- Héros ---
                var mage     = new Hero { Name = "Mage",     Health = 80  };
                var warrior  = new Hero { Name = "Warrior",  Health = 120 };
                var assassin = new Hero { Name = "Assassin", Health = 90  };

                db.Heroes.AddRange(mage, warrior, assassin);

                // --- Login ---
                var login = new Login
                {
                    Username     = "admin",
                    PasswordHash = PasswordHelper.HashPassword("admin123")
                };
                db.Logins.Add(login);

                db.SaveChanges(); // ← on sauvegarde pour avoir les IDs

                // --- Player ---
                var player = new Player { Name = "Player1", LoginID = login.ID };
                db.Players.Add(player);

                db.SaveChanges(); // ← on sauvegarde pour avoir l'ID du player

                // --- HeroSpell ---
                db.HeroSpells.AddRange(
                    new HeroSpell { HeroID = mage.ID,     SpellID = fireball.ID },
                    new HeroSpell { HeroID = mage.ID,     SpellID = frostbolt.ID },
                    new HeroSpell { HeroID = mage.ID,     SpellID = arcaneBlast.ID },
                    new HeroSpell { HeroID = mage.ID,     SpellID = blink.ID },

                    new HeroSpell { HeroID = warrior.ID,  SpellID = slash.ID },
                    new HeroSpell { HeroID = warrior.ID,  SpellID = shieldBash.ID },
                    new HeroSpell { HeroID = warrior.ID,  SpellID = warcry.ID },
                    new HeroSpell { HeroID = warrior.ID,  SpellID = execute.ID },

                    new HeroSpell { HeroID = assassin.ID, SpellID = shadowStrike.ID },
                    new HeroSpell { HeroID = assassin.ID, SpellID = poisonBlade.ID },
                    new HeroSpell { HeroID = assassin.ID, SpellID = smokeScreen.ID },
                    new HeroSpell { HeroID = assassin.ID, SpellID = backstab.ID }
                );

                // --- PlayerHero ---
                db.PlayerHeroes.Add(new PlayerHero { PlayerID = player.ID, HeroID = warrior.ID });

                db.SaveChanges();

                MessageBox.Show("✅ Base de données initialisée avec succès !");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Erreur seed : " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}