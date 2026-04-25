using System;
using System.Windows;
using System.Windows.Controls;
using HeroArena.ViewModels;

namespace HeroArena.Views
{
    public partial class LoginView : Window
    {
        private LoginVMX _vm;

        public LoginView()
        {
            InitializeComponent();

            _vm = new LoginVMX();
            _vm.CloseAction = new Action(this.Close);

            DataContext = _vm;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _vm.Password = ((PasswordBox)sender).Password;
        }
    }
}