using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Models;
using MauiApp1.Models.Viewmodels;

namespace MauiApp1;

public partial class VehicleDetail : ContentPage
{
    private Vehicle vehicle;

    public VehicleDetail(Vehicle v)
    {
        vehicle = v;
        InitializeComponent();
    }

    private async void UpdateEntry(object sender, EventArgs e)
    {
        vehicle.BodyType = (VehicleType)VehicleBodyType.SelectedIndex;
        vehicle.FuelType = (FuelType)vehicleFuel.SelectedIndex;

        var newVehicle = (await App.Database.UpdateVehicle(vehicle));
        if (newVehicle == null)
        {
            await DisplayAlert("Invalid data", "Reg. number and VIN need to be unique.", "OK");
            return;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar("Entry updated.");

        await snackbar.Show(cancellationTokenSource.Token);

        BindingContext = newVehicle;
        vehicle = newVehicle;

    }

    private async void RemoveEntry(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Remove", "Do you really want to remove " + vehicle.Name + " ?", "Yes", "No");
        if (answer)
        {
            await App.Database.RemoveVehicle(vehicle.Id);
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar("Entry deleted.");

        await snackbar.Show(cancellationTokenSource.Token);

    }

    private ISnackbar MakeSnackbar(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
            ActionButtonTextColor = Colors.Yellow,
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
        };

        string text = message;
        string actionButtonText = "OK";
        Action action = async () => await Navigation.PopAsync();
        TimeSpan duration = TimeSpan.FromSeconds(3);

        var snackbar = Snackbar.Make(text, action, actionButtonText, duration, snackbarOptions);

        return snackbar;
    }
}