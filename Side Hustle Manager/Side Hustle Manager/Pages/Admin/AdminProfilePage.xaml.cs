using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Services;

namespace Side_Hustle_Manager.Pages.Admin;


public partial class AdminProfilePage : ContentPage
{
    private UserDatabaseService _db = App.UserDatabase;
    private AdminProfileModel _profile;
    private string _username => "admin";

    public AdminProfilePage()
    {
        InitializeComponent();
        LoadProfile();
        LoadRatings();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var username = AdminProfileTabbedPage.CurrentEmployerUsername;

        var profile = _db.GetAdminProfile(username);
        BindingContext = profile;

        var avg = _db.GetAverageRating(username);
        AverageRatingLabel.Text = $"Average rating: {avg:F1}/5";
    }


    private void LoadProfile()
    {
        _profile = _db.GetAdminProfile() ?? new AdminProfileModel();
        NameEntry.Text = _profile.Name;
        CompanyEntry.Text = _profile.CompanyName;
        AddressEntry.Text = _profile.Address;
        ContactEntry.Text = _profile.ContactInfo;

        if (!string.IsNullOrEmpty(_profile.ProfileImagePath))
            ProfileImage.Source = _profile.ProfileImagePath;
    }

    private void LoadRatings()
    {
        var ratings = _db.GetRatingsForUser(_username);
        RatingsCollection.ItemsSource = ratings;

        var avg = _db.GetAverageRating(_username);
        AverageRatingLabel.Text = $"Average: {avg:F1}/5";
    }

    private void OnSubmitRatingClicked(object sender, EventArgs e)
    {
        if (StarPicker.SelectedIndex == -1)
            return;

        var rating = new RatingModel
        {
            RatedByUsername = App.CurrentUser.Username,
            RatedToUsername = _username,
            Stars = StarPicker.SelectedIndex + 1,
            Comment = CommentEntry.Text
        };

        _db.AddRating(rating);

        DisplayAlert("Success", "Rating submitted!", "OK");

        LoadRatings();

        StarPicker.SelectedIndex = -1;
        CommentEntry.Text = "";
    }

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

    private void OnSaveClicked(object sender, EventArgs e)
    {
        _profile.Name = NameEntry.Text;
        _profile.CompanyName = CompanyEntry.Text;
        _profile.Address = AddressEntry.Text;
        _profile.ContactInfo = ContactEntry.Text;

        _db.SaveAdminProfile(_profile);
        DisplayAlert("Success", "Profile updated", "OK");
    }

   

    private async void Logout_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlertAsync("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (answer)
        {
            Application.Current!.Windows[0].Page = new NavigationPage(new LoginPage());
        }
    }
}