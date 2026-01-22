using Side_Hustle_Manager.Pages.User;

namespace Side_Hustle_Manager.Shells
{
    public partial class UserShell : Shell
    {
        public UserShell()
        {
            InitializeComponent();

            // Treći tab = profil
            var profileSection = this.Items[0].Items[2];
            if (profileSection != null)
            {
                profileSection.Items.Clear();
                profileSection.Items.Add(new ShellContent
                {
                    Content = new UserProfilePage() // bez parametra
                });
            }
        }
    }
}
