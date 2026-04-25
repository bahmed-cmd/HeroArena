using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HeroArena.Data;
using HeroArena.Services;
using HeroArena.Views;

namespace HeroArena.ViewModels
{
    public class LoginVMX : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool DebugFlag { get; set; } = true;

        private string _username = "";
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; set; }

        public Action? CloseAction { get; set; }

        public LoginVMX()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Remplis tous les champs");
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    string hash = PasswordHelper.HashPassword(Password);

                    var user = db.Logins
                        .FirstOrDefault(u => u.Username == Username && u.PasswordHash == hash);

                    if (user != null)
                    {
                        MessageBox.Show("Connexion réussie");

                        UserSession.UserId = user.ID;

                        var main = new MainView();
                        main.Show();

                        CloseAction?.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Identifiants incorrects");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}