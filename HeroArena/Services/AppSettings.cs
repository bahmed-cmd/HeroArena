using System;
using System.IO;
using System.Text.Json;

namespace HeroArena.Services
{
    public class AppSettings
    {
        private static string _filePath = "appsettings.json";

        public string ConnectionString { get; set; } =
            "Server=localhost\\SQLEXPRESS01;Database=ExerciceHero;Trusted_Connection=True;TrustServerCertificate=True;";

        // 🔥 Singleton (une seule instance dans toute l'app)
        private static AppSettings _instance;

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                    Load();

                return _instance;
            }
        }

        // 📥 Charger depuis fichier JSON
        public static void Load()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _instance = JsonSerializer.Deserialize<AppSettings>(json);
                }
                else
                {
                    _instance = new AppSettings();
                    Save(); // créer le fichier par défaut
                }
            }
            catch
            {
                _instance = new AppSettings();
            }
        }

        // 💾 Sauvegarder dans JSON
        public static void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(Instance, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur sauvegarde settings : " + ex.Message);
            }
        }
    }
}