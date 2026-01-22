using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Services;

namespace Side_Hustle_Manager.Pages.Admin;

public partial class AdminRatingsPage : ContentPage
{
    private readonly UserDatabaseService _db = App.UserDatabase;

    public AdminRatingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRatings();
    }

    private void LoadRatings()
    {
        var username = AdminProfileTabbedPage.CurrentEmployerUsername;
        RatingsCollection.ItemsSource = _db.GetRatingsForUser(username);

        var avg = _db.GetAverageRating(username);
        AverageRatingLabel.Text = $"Average: {avg:F1}/5";
    }

    private void OnSubmitRatingClicked(object sender, EventArgs e)
    {
        if (StarPicker.SelectedIndex == -1)
            return;

        var rating = new RatingModel
        {
            RatedByUsername = App.CurrentUser.Username,
            RatedToUsername = AdminProfileTabbedPage.CurrentEmployerUsername,
            Stars = StarPicker.SelectedIndex + 1,
            Comment = CommentEntry.Text
        };

        _db.AddRating(rating);
        LoadRatings();

        StarPicker.SelectedIndex = -1;
        CommentEntry.Text = "";
    }
}
