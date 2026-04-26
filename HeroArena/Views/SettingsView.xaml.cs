using System.Windows;
using HeroArena.ViewModels;

namespace HeroArena.Views
{
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = new SettingsVMX();
        }
    }
}
