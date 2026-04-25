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
    public class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // ===== LISTE DES HEROS =====
        public ObservableCollection<Hero> Heroes { get; set; }

        // ===== HERO SELECTIONNÉ =====
        private Hero? _selectedHero;
        public Hero? SelectedHero
        {
            get => _selectedHero;
            set
            {
                _selectedHero = value;
                OnPropertyChanged(nameof(SelectedHero));
            }
        }

        // ===== COMMANDE =====
        public ICommand SelectHeroCommand { get; set; }

        public MainVM()
        {
            Heroes = new ObservableCollection<Hero>();
            SelectHeroCommand = new RelayCommand(SelectHero);

            LoadHeroes();
        }

        private void LoadHeroes()
        {
            using (var db = new AppDbContext())
            {
                var heroesFromDb = db.Heroes.ToList();

                foreach (var hero in heroesFromDb)
                {
                    Heroes.Add(hero);
                }
            }
        }

        private void SelectHero()
        {
            if (SelectedHero == null)
            {
                MessageBox.Show("Choisis un héros !");
                return;
            }

            using (var db = new AppDbContext())
            {
                var exists = db.PlayerHeroes
                    .Any(ph => ph.PlayerID == UserSession.UserId &&
                               ph.HeroID == SelectedHero.ID);

                if (exists)
                {
                    MessageBox.Show("Tu as déjà ce héros !");
                    return;
                }

                var playerHero = new PlayerHero
                {
                    PlayerID = UserSession.UserId,
                    HeroID = SelectedHero.ID
                };

                db.PlayerHeroes.Add(playerHero);
                db.SaveChanges();
            }

            MessageBox.Show($"🔥 {SelectedHero.Name} ajouté à ton compte !");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}