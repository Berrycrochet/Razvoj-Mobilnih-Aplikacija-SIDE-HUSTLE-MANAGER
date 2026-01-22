using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Pages.Admin;
using System.Collections.ObjectModel;

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

        var sideHustles = await App.SideHustleDatabase.GetUserSideHustlesAsync("1"); // zamijeni "1" sa pravim user id
        foreach (var item in sideHustles)
        {
            MySideHustles.Add(item);
        }
    }

    private async void OnSideHustleTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        if (frame == null)
            return;

        var sideHustle = frame.BindingContext as UserSideHustleViewModel;
        if (sideHustle == null)
            return;

        await Navigation.PushAsync(
            new AdminProfileTabbedPage(sideHustle.EmployerUsername)
        );
    }
}
