using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Pages.User;
using Side_Hustle_Manager.Services;
using Side_Hustle_Manager.Shells;
using Microsoft.Maui.Media;       // za pick / camera
using Microsoft.Maui.ApplicationModel; // za permissions
using SQLite;
using System.ComponentModel.Design;


namespace Side_Hustle_Manager.Pages;

public partial class LoginPage : ContentPage
{
    public LoginModel LoginData { get; set; }

    public LoginPage()
    {
        InitializeComponent();

        LoginData = new LoginModel();
        BindingContext = LoginData;
    }
    private UserDatabaseService _db => App.UserDatabase;

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var user = App.UserDatabase.GetUser(LoginData.Username, LoginData.Password);
        if (user != null)
        {
            App.CurrentUser = user; // spremi prijavljenog korisnika
            if (user.IsAdmin)
                Application.Current.MainPage = new AdminShell();
            else
                Application.Current.MainPage = new UserShell();
        }
        else
        {
            await DisplayAlertAsync("Greska", "Pogrešno korisničko ime ili lozinka.", "OK");
        }
    }
    


    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}




