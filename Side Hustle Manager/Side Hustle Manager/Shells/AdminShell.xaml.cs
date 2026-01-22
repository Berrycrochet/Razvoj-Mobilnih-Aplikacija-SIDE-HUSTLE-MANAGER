using Side_Hustle_Manager.Pages.Admin;
namespace Side_Hustle_Manager.Shells;

public partial class AdminShell : Shell
{
	public AdminShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AdminAddJobPage), typeof(AdminAddJobPage));
		Routing.RegisterRoute(nameof(AdminApplicationPage), typeof(AdminApplicationPage));
    }
}