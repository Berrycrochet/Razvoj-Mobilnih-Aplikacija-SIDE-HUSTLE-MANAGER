using Microsoft.Extensions.DependencyInjection;
using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Pages;
using Side_Hustle_Manager.Services;

namespace Side_Hustle_Manager
{
    public partial class App : Application
    {
        // ---------------- Globalni pristup bazama ----------------
        public static UserDatabaseService UserDatabase { get; private set; }
        public static SideHustleDatabaseService SideHustleDatabase { get; private set; }

        public static UserModel CurrentUser { get; set; }

        public App()
        {
            InitializeComponent();

            // ---------------- Inicijalizacija baza ----------------
            // Tvoja baza (korisnici)
            UserDatabase = new UserDatabaseService();

            // Prijateljičina baza (Side Hustles + Applications)
            var shmDbPath = Path.Combine(FileSystem.AppDataDirectory, "shm.db3");
            SideHustleDatabase = new SideHustleDatabaseService(shmDbPath);

            // ---------------- Startna stranica ----------------
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
