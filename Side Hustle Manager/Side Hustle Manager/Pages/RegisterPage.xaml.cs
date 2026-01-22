using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Services;
using System.Text.RegularExpressions;

namespace Side_Hustle_Manager.Pages;

public partial class RegisterPage : ContentPage
{
    private UserDatabaseService _db;
    public UserModel RegisterData { get; set; }

    public RegisterPage()
    {
        InitializeComponent();
        _db = App.UserDatabase;
        RegisterData = new UserModel();
        BindingContext = RegisterData;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (!IsValidEmail(RegisterData.Username))
        {
            await DisplayAlertAsync("Greška", "Unesi te validan račun.", "OK");
            return;
        }

        if (RegisterData.Password.Length < 8)
        {
            await DisplayAlertAsync("Greška", "Lozinka treba da ima najmanje 8 karaktera.", "OK");
            return;
        }

        if (_db.UserExists(RegisterData.Username))
        {
            await DisplayAlertAsync("Greška", "Korisnik već postoji.", "OK");
            return;
        }

        RegisterData.IsAdmin = AdminRadio.IsChecked;
        _db.AddUser(RegisterData);

        await DisplayAlertAsync("Uspješno", "Korisnički račun je napravljen!", "OK");
        await Navigation.PopAsync();
    }

    private bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) &&
               Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
