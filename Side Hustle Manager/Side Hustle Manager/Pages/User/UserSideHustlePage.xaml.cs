using Side_Hustle_Manager.Models;
using System.Collections.ObjectModel;
using Side_Hustle_Manager.Pages.Admin;


namespace Side_Hustle_Manager.Pages.User;




public partial class UserSideHustlePage : ContentPage
{
    public ObservableCollection<UserSideHustleViewModel> MySideHustles { get; set; } = new();

    public UserSideHustlePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        MySideHustles.Clear();

        var sideHustles = await App.SideHustleDatabase.GetUserSideHustlesAsync("1"); // vra?a List<UserSideHustleViewModel>

        foreach (var item in sideHustles)
        {
            MySideHustles.Add(item);
        }
    }

    private async void OnSideHustleTapped(object sender, EventArgs e)
    {
        var tap = (TappedEventArgs)e;
        var sideHustle = tap.Parameter as UserSideHustleViewModel;

        if (sideHustle == null)
            return;

        // EmployerUsername mora postojati u modelu
        await Navigation.PushAsync(
            new AdminProfileTabbedPage(sideHustle.EmployerUsername)
        );
    }

}