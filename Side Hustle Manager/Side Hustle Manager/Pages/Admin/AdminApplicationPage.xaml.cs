using Side_Hustle_Manager.Models;

using System.Collections.ObjectModel;

namespace Side_Hustle_Manager.Pages.Admin;

[QueryProperty(nameof(SideHustleId), "SideHustleId")]
public partial class AdminApplicationPage : ContentPage
{
    public ObservableCollection<JobApplicationModel> Applications { get; set; } = new();

    public int SideHustleId { get; set; }

    public AdminApplicationPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Applications.Clear();
        var apps = await App.SideHustleDatabase.GetApplicationsForJobAsync(SideHustleId);

        foreach (var a in apps)
            Applications.Add(a);
    }

    private async void Accept_Clicked(object sender, EventArgs e)
    {
        var app = (JobApplicationModel)((Button)sender).BindingContext;
        app.Status = "Accepted";

        await App.SideHustleDatabase.SaveApplicationAsync(app);
        await DisplayAlertAsync("Završeno", "Prijava je prihvacena.", "OK");
    }

    private async void Reject_Clicked(object sender, EventArgs e)
    {
        var app = (JobApplicationModel)((Button)sender).BindingContext;
        app.Status = "Rejected";

        await App.SideHustleDatabase.SaveApplicationAsync(app);
        await DisplayAlertAsync("Završeno", "Prijava je odbijena.", "OK");
    }
}


