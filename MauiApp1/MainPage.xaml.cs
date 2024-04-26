using MauiApp1.Models;
using MauiApp1.Models.VisualModels;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            LoadVehicles();

        }

        private async Task LoadVehicles()
        {
            var vehicles = (await App.Database.GetAllVehicles());

            List<VehicleItem> items = new List<VehicleItem>();
            foreach (var vehicle in vehicles)
            {
                items.Add(new VehicleItem(vehicle.Name, $"{vehicle.Make} {vehicle.Model}"));
            }

            VehiclesListView.ItemsSource = items;            
        }

        private async void AddVehicle(Object sevder, EventArgs args)
        {
            var vehicle = new Vehicle("Seat", "Seat", "Altea", 2009, "MI3654FFFO", "eeeepey", 0, 0, 0, VehicleType.MPV, FuelType.LPG, 600);
            await Task.Delay(8000);
            await App.Database.AddNewVehicle(vehicle);
            await LoadVehicles();
        }
    }


}
