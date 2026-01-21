using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Services;
using Side_Hustle_Manager.Shells; 

namespace Side_Hustle_Manager.Pages
{
    public partial class RegisterPage : ContentPage
{
    private UserDatabaseService _db;
    public UserModel RegisterData { get; set; }

    public RegisterPage()
    {
        InitializeComponent();
        _db = App.UserDatabase; ;
        RegisterData = new UserModel();
        BindingContext = RegisterData;
    }
    private void OnUserChecked(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
            RegisterData.IsAdmin = false;
    }
    private void OnAdminChecked(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
            RegisterData.IsAdmin = true;
    }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RegisterData.Username) || string.IsNullOrWhiteSpace(RegisterData.Password))
            {
                await DisplayAlertAsync("Error", "Please fill all fields", "OK");
                return;
            }

            if (_db.UserExists(RegisterData.Username))
            {
                await DisplayAlertAsync("Error", "User already exists", "OK");
                return;
            }

            // Ovdje odredi tip korisnika na osnovu RadioButton-a
            RegisterData.IsAdmin = AdminRadio.IsChecked;

            _db.AddUser(RegisterData);

            await DisplayAlertAsync("Success", "User registered!", "OK");
            await Navigation.PopAsync(); // vraća na login
        }
    }
}