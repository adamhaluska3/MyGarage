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
            await DisplayAlert("Alert", "Invalid data", "OK");
            return;
        }

        BindingContext = newVehicle;
        vehicle = newVehicle;

    }
}