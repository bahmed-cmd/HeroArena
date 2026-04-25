using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HeroArena.Data;
using HeroArena.Models;
using HeroArena.Services;

namespace HeroArena.ViewModels
{
    public class CombatVMX : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // 🔥 OBLIGATOIRE
        public bool DebugFlag { get; set; } = true;

        public const int HP_MAX_WHEN_DIE = 12;

        // ===== HERO JOUEUR =====
        public Hero PlayerHero { get; set; }
        public int PlayerHP { get; set; }

        // ===== ENNEMI =====
        public Hero EnemyHero { get; set; }
        public int EnemyHP { get; set; }

        // ===== SPELLS =====
        public ObservableCollection<Spell> PlayerSpells { get; set; } = new();

        // ===== SCORE =====
        private int _score;
        public int Score
        {
            get => _score;
            set { _score = value; OnPropertyChanged(nameof(Score)); }
        }

        // ===== COMMAND =====
        public ICommand AttackCommand { get; set; }
        public ICommand RestartCommand { get; set; }

        public CombatVMX()
        {
            AttackCommand = new RelayCommand(Attack);
            RestartCommand = new RelayCommand(Restart);

            LoadPlayerHero();
            GenerateEnemy();
        }

        // ===== LOAD HERO JOUEUR =====
        private void LoadPlayerHero()
        {
            using (var db = new AppDbContext())
            {
                var hero = db.PlayerHeroes
                    .Where(ph => ph.PlayerID == UserSession.UserId)
                    .Join(db.Heroes,
                        ph => ph.HeroID,
                        h => h.ID,
                        (ph, h) => h)
                    .FirstOrDefault();

                if (hero == null)
                {
                    MessageBox.Show("Aucun héros sélectionné !");
                    return;
                }

                PlayerHero = hero;
                PlayerHP = hero.Health;

                LoadPlayerSpells(hero.ID);
            }
        }

        // ===== LOAD SPELLS =====
        private void LoadPlayerSpells(int heroId)
        {
            using (var db = new AppDbContext())
            {
                var spells = db.HeroSpells
                    .Where(hs => hs.HeroID == heroId)
                    .Join(db.Spells,
                        hs => hs.SpellID,
                        s => s.ID,
                        (hs, s) => s)
                    .ToList();

                PlayerSpells.Clear();
                foreach (var s in spells)
                    PlayerSpells.Add(s);
            }
        }

        // ===== GENERATE ENEMY =====
        private void GenerateEnemy()
        {
            using (var db = new AppDbContext())
            {
                var rnd = new Random();
                var enemy = db.Heroes.OrderBy(x => Guid.NewGuid()).First();

                EnemyHero = new Hero
                {
                    ID = enemy.ID,
                    Name = enemy.Name + " (Enemy)",
                    Health = (int)(enemy.Health * 1.1) // +10%
                };

                EnemyHP = EnemyHero.Health;
                OnPropertyChanged(nameof(EnemyHP));
            }
        }

        // ===== ATTACK =====
        private void Attack()
        {
            if (PlayerSpells.Count == 0) return;

            var spell = PlayerSpells.First(); // simplifié

            EnemyHP -= spell.Damage;

            if (EnemyHP <= 0)
            {
                Score++;
                MessageBox.Show($"🔥 Tu as vaincu {EnemyHero.Name} !");
                GenerateEnemy();
                return;
            }

            // attaque ennemie simple
            PlayerHP -= 10;

            if (PlayerHP <= 0)
            {
                MessageBox.Show($"💀 {PlayerHero.Name} est mort...");
                PlayerHP = HP_MAX_WHEN_DIE;
            }

            OnPropertyChanged(nameof(PlayerHP));
            OnPropertyChanged(nameof(EnemyHP));
        }

        // ===== RESTART =====
        private void Restart()
        {
            PlayerHP = PlayerHero.Health;
            GenerateEnemy();
            OnPropertyChanged(nameof(PlayerHP));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}