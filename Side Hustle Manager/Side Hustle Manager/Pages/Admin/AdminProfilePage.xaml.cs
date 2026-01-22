using Microsoft.Maui.Media;
using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Services;
using System.Xml;

namespace Side_Hustle_Manager.Pages.Admin;

public partial class AdminProfilePage : ContentPage
{
    private UserDatabaseService _db = App.UserDatabase;
    private AdminProfileModel _profile;

    public bool IsEditMode { get; set; } = false;

    public AdminProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadProfile();
    }

    private void LoadProfile()
    {
        _profile = _db.GetAdminProfile() ?? new AdminProfileModel();

        // Read-only prikaz
        NameLabel.Text = _profile.Name;
        CompanyLabel.Text = _profile.CompanyName;
        AddressLabel.Text = _profile.Address;
        ContactLabel.Text = _profile.ContactInfo;

        // Edit polja
        NameEntry.Text = _profile.Name;
        CompanyEntry.Text = _profile.CompanyName;
        AddressEntry.Text = _profile.Address;
        ContactEntry.Text = _profile.ContactInfo;

        if (!string.IsNullOrEmpty(_profile.ProfileImagePath))
            ProfileImage.Source = _profile.ProfileImagePath;
    }

    // -------- EDIT MODE --------
    private void OnEditClicked(object sender, EventArgs e)
    {
        IsEditMode = true;
        ToggleEdit(true);
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        _profile.Name = NameEntry.Text;
        _profile.CompanyName = CompanyEntry.Text;
        _profile.Address = AddressEntry.Text;
        _profile.ContactInfo = ContactEntry.Text;

        _db.SaveAdminProfile(_profile);

        IsEditMode = false;
        ToggleEdit(false);
        LoadProfile();
    }

    private void OnCancelEditClicked(object sender, EventArgs e)
    {
        IsEditMode = false;
        ToggleEdit(false);
        LoadProfile();
    }

    private void ToggleEdit(bool show)
    {
        EditProfileButton.IsVisible = !show;
        SaveProfileButton.IsVisible = show;
        CancelEditButton.IsVisible = show;

        ChangeImageButton.IsVisible = show;

        NameEntry.IsVisible = show;
        CompanyEntry.IsVisible = show;
        AddressEntry.IsVisible = show;
        ContactEntry.IsVisible = show;

        NameLabel.IsVisible = !show;
        CompanyLabel.IsVisible = !show;
        AddressLabel.IsVisible = !show;
        ContactLabel.IsVisible = !show;
    }

    // -------- IMAGE --------
    private async void OnChangeImageClicked(object sender, EventArgs e)
    {
        try
        {
            string action = await DisplayActionSheet("Select Image", "Cancel", null, "Gallery", "Camera");
            FileResult photo = null;

            if (action == "Gallery")
                photo = await MediaPicker.PickPhotoAsync();
            else if (action == "Camera")
                photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                ProfileImage.Source = ImageSource.FromStream(() => stream);
                _profile.ProfileImagePath = photo.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // -------- LOGOUT --------
    private async void Logout_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Logout", "Are you sure?", "Yes", "No");
        if (answer)
            Application.Current!.Windows[0].Page = new NavigationPage(new LoginPage());
    }
}
