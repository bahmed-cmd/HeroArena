using System.Windows;
using HeroArena.ViewModels;

namespace HeroArena.Views
{
    public partial class CombatView : Window
    {
        public CombatView()
        {
            InitializeComponent();
            DataContext = new CombatVMX();
        }
    }
}