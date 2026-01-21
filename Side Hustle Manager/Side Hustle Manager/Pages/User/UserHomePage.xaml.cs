using Side_Hustle_Manager.Models;
using System.Collections.ObjectModel;

namespace Side_Hustle_Manager.Pages.User;


public partial class UserHomePage : ContentPage
{
    public ObservableCollection<SideHustleModel> SideHustles { get; set; } = new();
    private List<SideHustleModel> _allSideHustles = new();

    public UserHomePage()
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

    private async void Apply_Clicked(object sender, EventArgs e)
    {
        var job = (sender as Button)?.CommandParameter as SideHustleModel;
        if (job == null) return;

        await App.SideHustleDatabase.ApplyToJobAsync(new JobApplicationModel
        {
            SideHustleId = job.Id,
            UserId = "1"
        });

        await DisplayAlertAsync("Prijavljen/a", "Prijavili ste se na posao.", "OK");
    }
}