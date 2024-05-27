using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MyGarage.Models;
using MyGarage.Resources.Languages;

namespace MyGarage.Views;

public partial class VehicleDetail : ContentPage
{
    private Vehicle _vehicle;

    public VehicleDetail(Vehicle v)
    {
        _vehicle = v;
        InitializeComponent();

        if (_vehicle.Id == -1)
            DeleteEntry.IsEnabled = false;

        if (_vehicle.Name == null)
        {
            Title = LangRes.NewVehicle;
        }

        LoadPickers();
    }

    protected override void OnAppearing()
    {
        LoadBindings();
        base.OnAppearing();
    }

    private void LoadBindings()
    {
        BindingContext = _vehicle;
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

        var fuelOptions = new List<string>()
        {
            LangRes.Petrol,
            LangRes.Diesel,
            LangRes.LPG,
            LangRes.Electric

        };

        VehicleFuel.ItemsSource = fuelOptions;
    }


    // Event handlers
    private async void UpdateEntry(object sender, EventArgs e)
    {
        try
        {
            _vehicle.Name = _vehicle.Name.Trim();
            _vehicle.Make = _vehicle.Make.Trim();
            _vehicle.Model = _vehicle.Model.Trim();
            _vehicle.RegNumber = _vehicle.RegNumber.Trim();

            if (_vehicle.Name == "" || _vehicle.Make == "" || _vehicle.Model == "" || _vehicle.RegNumber == "")
                throw new NullReferenceException();
        }
        catch (NullReferenceException)
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.EmptyFields, "OK");
            return;
        }

        if (_vehicle.Year < Constants.LowestYearAllowed || _vehicle.Year > DateTime.Now.Year)
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.InvalidYear, "OK");
            return;
        }

        _vehicle.ImageSource = (_vehicle.BodyType).ToString().ToLowerInvariant() + ".png";

        var newVehicle = await App.Database.UpdateVehicle(_vehicle);
        if (newVehicle == null)
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.UniqRegVIN, "OK");
            return;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = Utilities.MakeSnackbar(LangRes.EntryUpdated, Navigation);

        await snackbar.Show(cancellationTokenSource.Token);

        BindingContext = newVehicle;
        _vehicle = newVehicle;
        Title = newVehicle.Name;
        DeleteEntry.IsEnabled = true;
    }

    private async void RemoveEntry(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(LangRes.Remove, LangRes.ReallyRemoveVehicle + _vehicle.Name + " ?", LangRes.Yes, LangRes.No);
        if (!answer)
            return;

        await App.Database.RemoveVehicle(_vehicle.Id);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = Utilities.MakeSnackbar(LangRes.EntryDeleted, Navigation);

        await snackbar.Show(cancellationTokenSource.Token);

        await Navigation.PopAsync();
    }
}