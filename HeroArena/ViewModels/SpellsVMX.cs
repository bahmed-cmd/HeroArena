using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using HeroArena.Data;
using HeroArena.Models;

namespace HeroArena.ViewModels
{
    public class SpellsVMX : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // 🔥 OBLIGATOIRE (consigne projet)
        public bool DebugFlag { get; set; } = true;

        // ===== LISTES =====
        public ObservableCollection<Spell> Spells { get; set; } = new();
        public ObservableCollection<Hero> Heroes { get; set; } = new();

        // ===== SELECTION SPELL =====
        private Spell? _selectedSpell;
        public Spell? SelectedSpell
        {
            get => _selectedSpell;
            set
            {
                _selectedSpell = value;
                OnPropertyChanged(nameof(SelectedSpell));
            }
        }

        // ===== SELECTION HERO (FILTRE) =====
        private Hero? _selectedHero;
        public Hero? SelectedHero
        {
            get => _selectedHero;
            set
            {
                _selectedHero = value;
                OnPropertyChanged(nameof(SelectedHero));
                FilterSpells();
            }
        }

        // ===== CONSTRUCTEUR =====
        public SpellsVMX()
        {
            LoadHeroes();
            LoadSpells();
        }

        // ===== CHARGER HEROES =====
        private void LoadHeroes()
        {
            using (var db = new AppDbContext())
            {
                Heroes.Clear();
                var heroes = db.Heroes.ToList();

                foreach (var hero in heroes)
                {
                    Heroes.Add(hero);
                }
            }
        }

        // ===== CHARGER TOUS LES SPELLS =====
        private void LoadSpells()
        {
            using (var db = new AppDbContext())
            {
                Spells.Clear();
                var spells = db.Spells.ToList();

                foreach (var spell in spells)
                {
                    Spells.Add(spell);
                }
            }
        }

        // ===== FILTRE PAR HERO =====
        private void FilterSpells()
        {
            using (var db = new AppDbContext())
            {
                Spells.Clear();

                // Si aucun héros sélectionné → afficher tous les spells
                if (SelectedHero == null)
                {
                    var allSpells = db.Spells.ToList();
                    foreach (var spell in allSpells)
                    {
                        Spells.Add(spell);
                    }
                    return;
                }

                // Sinon → filtre via HeroSpell
                var filteredSpells = db.HeroSpells
                    .Where(hs => hs.HeroID == SelectedHero.ID)
                    .Join(db.Spells,
                          hs => hs.SpellID,
                          s => s.ID,
                          (hs, s) => s)
                    .ToList();

                foreach (var spell in filteredSpells)
                {
                    Spells.Add(spell);
                }
            }
        }

        // ===== NOTIFICATION UI =====
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}