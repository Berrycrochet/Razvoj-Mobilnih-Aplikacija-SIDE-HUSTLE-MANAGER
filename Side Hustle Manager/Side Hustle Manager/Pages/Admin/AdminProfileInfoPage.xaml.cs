using Side_Hustle_Manager.Services;

namespace Side_Hustle_Manager.Pages.Admin;

public partial class AdminProfileInfoPage : ContentPage
{
    private readonly UserDatabaseService _db = App.UserDatabase;

    public AdminProfileInfoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var username = AdminProfileTabbedPage.CurrentEmployerUsername;

        var profile = _db.GetAdminProfile(username);
        if (profile == null)
            return;

        BindingContext = profile;

        if (!string.IsNullOrWhiteSpace(profile.ProfileImagePath))
            ProfileImage.Source = profile.ProfileImagePath;

        var avg = _db.GetAverageRating(username);
        AverageRatingLabel.Text = $"Average rating: {avg:F1}/5";
    }
}
