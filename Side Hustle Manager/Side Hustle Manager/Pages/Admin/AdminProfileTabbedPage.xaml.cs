using Side_Hustle_Manager.Pages.Admin;

namespace Side_Hustle_Manager.Pages.Admin;

public partial class AdminProfileTabbedPage : TabbedPage
{
    public static string CurrentEmployerUsername;

    public AdminProfileTabbedPage(string username)
    {
        InitializeComponent();

        // Spremimo username
        CurrentEmployerUsername = username;

        // Dodajemo tabove programatski
        this.Children.Add(new AdminProfileInfoPage());
        this.Children.Add(new AdminRatingsPage());
    }
}
