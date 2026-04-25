using System.Windows;
using HeroArena.ViewModels;

namespace HeroArena.Views
{
    public partial class SpellsView : Window
    {
        public SpellsView()
        {
            InitializeComponent();
            DataContext = new SpellsVMX();
        }
    }
}