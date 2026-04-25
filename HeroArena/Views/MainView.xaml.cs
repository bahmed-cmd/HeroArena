using System.Windows;
using HeroArena.ViewModels;
using HeroArena.Views;

namespace HeroArena.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }

        private void OpenSpells(object sender, RoutedEventArgs e)
        {
            var spellsWindow = new SpellsView();
            spellsWindow.Show();
        }

        private void OpenCombat(object sender, RoutedEventArgs e)
        {
            var combatWindow = new CombatView();
            combatWindow.Show();
        }
    }
}