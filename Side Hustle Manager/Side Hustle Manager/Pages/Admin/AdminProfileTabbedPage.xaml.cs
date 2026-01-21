namespace Side_Hustle_Manager.Pages.Admin;

public partial class AdminProfileTabbedPage : TabbedPage
{
    public static string CurrentEmployerUsername;

    public AdminProfileTabbedPage(string employerUsername)
    {
        InitializeComponent();
        CurrentEmployerUsername = employerUsername;
    }
}
