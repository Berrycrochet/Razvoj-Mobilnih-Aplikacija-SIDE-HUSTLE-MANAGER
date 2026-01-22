using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Shells;
using Side_Hustle_Manager.Services;
using System.Text.RegularExpressions;

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

    private bool _isPasswordVisible = false;

    private void OnTogglePassword(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordEntry.IsPassword = !_isPasswordVisible;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // VALIDACIJA
        if (string.IsNullOrWhiteSpace(LoginData.Username) || string.IsNullOrWhiteSpace(LoginData.Password))
        {
            await DisplayAlertAsync("Greška", "Popunite sva polja.", "OK");
            return;
        }

        // Email validacija
        if (!Regex.IsMatch(LoginData.Username, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlertAsync("Greška", "Unesite validan email.", "OK");
            return;
        }

        // Lozinka minimalno 8 karaktera
        if (LoginData.Password.Length < 8)
        {
            await DisplayAlertAsync("Greška", "Lozinka mora imati najmanje 8 karaktera.", "OK");
            return;
        }

        var user = App.UserDatabase.GetUser(LoginData.Username, LoginData.Password);

        if (user != null)
        {
            App.CurrentUser = user;

            if (user.IsAdmin)
                Application.Current.MainPage = new AdminShell();
            else
                Application.Current.MainPage = new UserShell();
        }
        else
        {
            await DisplayAlert("Greška", "Pogrešan email ili lozinka.", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}
