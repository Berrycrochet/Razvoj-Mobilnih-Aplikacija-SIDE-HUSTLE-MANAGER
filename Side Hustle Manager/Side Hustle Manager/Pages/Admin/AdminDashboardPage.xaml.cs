using Side_Hustle_Manager.Models;
using System.Collections.ObjectModel;

namespace Side_Hustle_Manager.Pages.Admin;


public partial class AdminDashboardPage : ContentPage
{
    public ObservableCollection<SideHustleModel> SideHustles { get; set; } = new();
    private List<SideHustleModel> _allSideHustles = new();

    public AdminDashboardPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var hustles = await App.SideHustleDatabase.GetSideHustlesAsync();
        _allSideHustles = hustles;

        RefreshList(_allSideHustles);
        LoadCategories();
        LoadEmployers();
    }

    private void LoadCategories()
    {
        var categories = _allSideHustles
            .Select(h => h.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        categories.Insert(0, "Sve");
        CategoryPicker.ItemsSource = categories;
        CategoryPicker.SelectedIndex = 0;
    }

    private void LoadEmployers()
    {
        var employers = _allSideHustles
            .Select(h => h.EmployerName)
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Distinct()
            .OrderBy(e => e)
            .ToList();

        employers.Insert(0, "Svi");
        EmployerPicker.ItemsSource = employers;
        EmployerPicker.SelectedIndex = 0;
    }

    private void FilterChanged(object sender, EventArgs e)
    {
        var category = CategoryPicker.SelectedItem?.ToString();
        var employer = EmployerPicker.SelectedItem?.ToString();

        IEnumerable<SideHustleModel> filtered = _allSideHustles;

        if (category != "Sve")
            filtered = filtered.Where(h => h.Category == category);

        if (employer != "Svi")
            filtered = filtered.Where(h => h.EmployerName == employer);

        RefreshList(filtered);
    }

    private void RefreshList(IEnumerable<SideHustleModel> list)
    {
        SideHustles.Clear();
        foreach (var h in list)
            SideHustles.Add(h);
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var hustle = (sender as Button)?.CommandParameter as SideHustleModel;
        if (hustle == null) return;

        bool confirm = await DisplayAlertAsync("Izbriši", $"Izbriši '{hustle.Title}'?", "Da", "Ne");
        if (!confirm) return;

        await App.SideHustleDatabase.DeleteSideHustleAsync(hustle);
        _allSideHustles.Remove(hustle);
        RefreshList(_allSideHustles);
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        var hustle = (sender as Button)?.CommandParameter as SideHustleModel;
        if (hustle == null) return;

        await Shell.Current.GoToAsync(nameof(AdminAddJobPage),
            new Dictionary<string, object> { { "SideHustle", hustle } });
    }

    private async void Applications_Clicked(object sender, EventArgs e)
    {
        var job = (sender as Button)?.CommandParameter as SideHustleModel;
        if (job == null) return;

        await Shell.Current.GoToAsync(nameof(AdminApplicationPage),
            new Dictionary<string, object> { { "SideHustleId", job.Id } });
    }
}

