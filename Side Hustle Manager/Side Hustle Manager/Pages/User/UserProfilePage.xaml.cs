using Side_Hustle_Manager.Models;
using Side_Hustle_Manager.Services;
using Microsoft.Maui.Media;       // za pick / camera
using Microsoft.Maui.ApplicationModel; // za permissions
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;
using System.Linq;

namespace Side_Hustle_Manager.Pages.User
{
    public partial class UserProfilePage : ContentPage
    {
        private UserDatabaseService _db = App.UserDatabase;
        
        // Uzmi username iz trenutno prijavljenog korisnika
        private string _username => App.CurrentUser.Username;
       

        // ObservableCollections za CollectionView
        private ObservableCollection<UserSkillModel> _skills;
        private ObservableCollection<UserExperienceModel> _experiences;

        public UserProfilePage()
        {
            InitializeComponent();

            

            // Inicijalizacija praznih kolekcija
            _skills = new ObservableCollection<UserSkillModel>();
            _experiences = new ObservableCollection<UserExperienceModel>();

            SkillsCollection.ItemsSource = _skills;
            ExperienceCollection.ItemsSource = _experiences;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            LoadProfileImage();
            LoadSkills();
            LoadExperiences();
            LoadLocation();
            LoadRatings();

        }

        #region --- Profile Image ---
        private async void OnChangeImageClicked(object sender, EventArgs e)
        {
            try
            {
                string action = await DisplayActionSheet("Select Image", "Cancel", null, "Gallery", "Camera");
                FileResult photo = null;

                if (action == "Gallery")
                    photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Pick a profile photo" });
                else if (action == "Camera")
                    photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions { Title = "Take a profile photo" });

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    ProfileImage.Source = ImageSource.FromStream(() => stream);

                    // Uzmi trenutnog korisnika
                    var user = App.CurrentUser;
                    if (user != null)
                    {
                        user.ProfileImagePath = photo.FullPath;
                        _db.UpdateUser(user); // koristi novu metodu UpdateUser

                        // Ažuriraj App.CurrentUser da sadrži novu putanju
                        App.CurrentUser = user;

                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void LoadProfileImage()
        {
            var user = App.CurrentUser;

            if (user != null && !string.IsNullOrEmpty(user.ProfileImagePath))
            {
                ProfileImage.Source = ImageSource.FromFile(user.ProfileImagePath);
            }
        }
        #endregion

        #region --- Skills ---
        private void LoadSkills()
        {
            var list = _db.GetSkills(_username);
            _skills.Clear();
            foreach (var skill in list)
                _skills.Add(skill);
        }

        private void LoadLocation()
        {
            var user = App.CurrentUser;
            if (!string.IsNullOrEmpty(user.Location))
            {
                LocationLabel.Text = $"Lokacija: {user.Location}";
            }
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
                RatedByUsername = App.CurrentUser.Username, // tko daje ocjenu
                RatedToUsername = _username,                // profil koji gledamo
                Stars = StarPicker.SelectedIndex + 1,
                Comment = CommentEntry.Text
            };

            _db.AddRating(rating);

            DisplayAlert("Success", "Rating submitted!", "OK");

            // Osvježi prikaz
            LoadRatings();

            // Reset UI
            StarPicker.SelectedIndex = -1;
            CommentEntry.Text = "";
        }


        private void OnAddSkillClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SkillEntry.Text))
            {
                var skill = new UserSkillModel
                {
                    UserUsername = _username,
                    SkillName = SkillEntry.Text
                };
                _db.AddSkill(skill);
                _skills.Add(skill);
                SkillEntry.Text = "";
            }
        }

        private void OnDeleteSkillClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is UserSkillModel skill)
            {
                _db.DeleteSkill(skill);
                _skills.Remove(skill);
            }
        }
        #endregion

        #region --- Experience ---
        private void LoadExperiences()
        {
            var list = _db.GetExperiences(_username);
            _experiences.Clear();
            foreach (var exp in list)
                _experiences.Add(exp);
        }

        private void OnAddExperienceClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ExperienceTitleEntry.Text) &&
                !string.IsNullOrWhiteSpace(ExperienceCompanyEntry.Text) &&
                !string.IsNullOrWhiteSpace(ExperiencePeriodEntry.Text))
            {
                var exp = new UserExperienceModel
                {
                    UserUsername = _username,
                    Title = ExperienceTitleEntry.Text,
                    Company = ExperienceCompanyEntry.Text,
                    
                };
                _db.AddExperience(exp);
                _experiences.Add(exp);

                ExperienceTitleEntry.Text = "";
                ExperienceCompanyEntry.Text = "";
                ExperiencePeriodEntry.Text = "";
            }
        }

        private void OnDeleteExperienceClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is UserExperienceModel exp)
            {
                _db.DeleteExperience(exp);
                _experiences.Remove(exp);
            }
        }
        #endregion

        #region --- Logout ---
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (answer)
                Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        #endregion


        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await LocationService.GetCurrentLocation();

                if (location == null)
                {
                    LocationLabel.Text = "Lokacija nije dostupna";
                    return;
                }

                var placemarks = await Geocoding.Default.GetPlacemarksAsync(
                    location.Latitude,
                    location.Longitude);

                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    string city = placemark.Locality;
                    string country = placemark.CountryName;
                    string fullLocation = $"{city}, {country}";

                    LocationLabel.Text = $"Lokacija: {fullLocation}";

                    // 🔒 SPREMANJE U BAZU
                    var user = App.CurrentUser;
                    user.Location = fullLocation;
                    _db.UpdateUser(user);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Greška", ex.Message, "OK");
            }
        }
    }

}
