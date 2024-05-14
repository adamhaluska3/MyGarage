using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MyGarage.Models;
using MyGarage.Resources.Languages;

namespace MyGarage;

public partial class VehicleDetail : ContentPage
{
    private Vehicle vehicle;

    public VehicleDetail(Vehicle v)
    {
        vehicle = v;
        InitializeComponent();

        if (vehicle.Id == -1)
            deleteEntry.IsEnabled = false;

        LoadPickers();
    }

    private void LoadPickers()
    {
        var options = new List<string>()
        {
            LangRes.Hatchback,
            LangRes.Sedan,
            LangRes.SUV,
            LangRes.Crossover,
            LangRes.Coupe,
            LangRes.Convertible,
            LangRes.Wagon,
            LangRes.Minivan,
            LangRes.Pickup,
            LangRes.Supercar
        };

        VehicleBodyType.ItemsSource = options;

        options = new List<string>()
        {
            LangRes.Petrol,
            LangRes.Diesel,
            LangRes.LPG,
            LangRes.Electric
        };

        vehicleFuel.ItemsSource = options;
    }

    private async void UpdateEntry(object sender, EventArgs e)
    {
        try
        {
            vehicle.Name = vehicle.Name.Trim();
            vehicle.Make = vehicle.Make.Trim();
            vehicle.Model = vehicle.Model.Trim();
            vehicle.RegNumber = vehicle.RegNumber.Trim();

            if (vehicle.Name == "" || vehicle.Make == "" || vehicle.Model == "" || vehicle.RegNumber == "")
                throw new NullReferenceException();
        }
        catch (NullReferenceException)
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.EmptyFields, "OK");
            return;
        }

        vehicle.ImageSource = ((VehicleType)vehicle.BodyType).ToString().ToLowerInvariant() + ".png";

        var newVehicle = await App.Database.UpdateVehicle(vehicle);
        if (newVehicle == null)
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.UniqRegVIN, "OK");
            return;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar(LangRes.EntryUpdated);

        await snackbar.Show(cancellationTokenSource.Token);

        BindingContext = newVehicle;
        vehicle = newVehicle;
        deleteEntry.IsEnabled = true;

    }

    private async void RemoveEntry(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(LangRes.Remove, LangRes.ReallyRemoveVehicle + vehicle.Name + " ?", LangRes.Yes, LangRes.No);
        if (!answer)
            return;

        await App.Database.RemoveVehicle(vehicle.Id);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar(LangRes.EntryDeleted);

        await snackbar.Show(cancellationTokenSource.Token);

        await Navigation.PopAsync();

    }
    protected override async void OnAppearing()
    {
        await LoadBindings();
        base.OnAppearing();
    }

    private async Task LoadBindings()
    {
        BindingContext = vehicle;
    }

    private ISnackbar MakeSnackbar(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
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