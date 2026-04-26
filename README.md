HeroArena
Application WPF de combat tour par tour entre héros, développée en C# / .NET 10 avec Entity Framework Core et SQL Server.
Packages NuGet

Microsoft.EntityFrameworkCore 10.0.6
Microsoft.EntityFrameworkCore.SqlServer 10.0.6
Microsoft.EntityFrameworkCore.Tools 10.0.6

Initialisation de la base de données

Exécuter le script SQL de l'énoncé pour créer la base ExerciceHero sur votre instance SQL Server
Lancer l'application
Aller dans Settings, modifier la connection string selon votre instance, cliquer Sauvegarder
Cliquer sur Initialiser la base de données pour insérer les données par défaut

Données insérées automatiquement :

3 héros : Mage (80 HP), Warrior (120 HP), Assassin (90 HP)
12 spells (4 par héros)
1 compte par défaut : admin / 123

Fonctionnement de l'application
Connexion
L'utilisateur se connecte avec un nom d'utilisateur et un mot de passe. Le mot de passe est hashé en SHA-256 avant d'être comparé à la base de données.
Gestion des héros
Liste tous les héros disponibles en base. Le joueur peut sélectionner un héros pour son compte. Les spells associés au héros sélectionné sont affichés.
Combat
Le joueur affronte un héros ennemi généré aléatoirement avec +10% de HP. Le combat est tour par tour : le joueur utilise un spell pour attaquer, l'ennemi riposte automatiquement. Quand un héros tombe à 0 HP, un message personnalisé s'affiche. Le score augmente à chaque ennemi vaincu. Un bouton permet de relancer un combat.
Settings
Permet de modifier la connection string à la volée et de réinitialiser la base de données avec les données par défaut.
