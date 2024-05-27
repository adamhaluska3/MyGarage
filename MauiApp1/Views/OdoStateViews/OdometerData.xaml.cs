using MyGarage.Models;
using MyGarage.Models.ViewModels;

namespace MyGarage.Views;

public partial class OdometerData : ContentPage
{
    private int vehicleId;
    public OdometerData(int vehicleId)
    {
        this.vehicleId = vehicleId;

        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        await LoadStates();
        await UpdateData();
        base.OnAppearing();
    }
    private async Task UpdateData()
    {
        // Total driven
        var entries = await App.Database.GetOdometerStates(vehicleId);
        if (entries == null || entries.Count() < 2)
        {
            OdoAverage.Text = "-.-";
            OdometerSum.Text = "-";
            return;
        }
        var distance = entries.Max(entry => entry.State) - entries.Min(entry => entry.State);

        OdometerSum.Text = $"{distance:#,##0}";

        // Average
        var dateDifference = entries.Max(entry => entry.DateTime) - entries.Min(entry => entry.DateTime);

        OdoAverage.Text = $"{((float)distance / dateDifference.Days) * 7:#,##0.##}";
    }

    private async Task LoadStates()
    {
        var entries = await App.Database.GetOdometerStates(vehicleId);
        SetEntriesSource(entries);
    }


    // Event handlers
    private async void NavigateOdometerStateDetail(object sender, TappedEventArgs e)
    {
        var entry = e.Parameter as OdometerState;
        var noteDetailPage = new OdoStateDetail(entry);

        await Navigation.PushAsync(noteDetailPage, true);
        return;
    }

    private async void AddOdometerState(object sender, EventArgs e)
    {
        var newEntry = new OdometerState(vehicleId);
        newEntry.DateTime = DateTime.Now;

        var noteDetailPage = new OdoStateDetail(newEntry);

        await Navigation.PushAsync(noteDetailPage, true);
    }

    private void SetEntriesSource(List<OdometerState> entries)
    {
        var items = new List<OdometerStateItem>();
        foreach (var entry in entries)
        {
            items.Add(new OdometerStateItem(entry));
        }

        StatesCollection.ItemsSource = items;
    }
}