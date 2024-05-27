using MyGarage.Models;
using MyGarage.Models.ViewModels;
using MyGarage.Views;

namespace MyGarage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadVehicles();
        }

        protected override async void OnAppearing()
        {
            await LoadVehicles();
            base.OnAppearing();
        }

        private async Task LoadVehicles()
        {
            var vehicles = (await App.Database.GetAllVehicles());

            List<VehicleItem> items = [];
            items.AddRange(vehicles.Select(vehicle => new VehicleItem(vehicle.Name, $"{vehicle.Make} {vehicle.Model}", vehicle)));

            VehiclesCollectionView.ItemsSource = items;
        }


        // Event handlers
        private async void CreateVehicle(object sender, EventArgs args)
        {
            var vehicle = new Vehicle();

            var vehicleDetailPage = new VehicleDetail(vehicle)
            {
                BindingContext = vehicle
            };

            await Navigation.PushAsync(vehicleDetailPage, true);
        }

        private async void Reload(object sender, EventArgs args)
        {
            await LoadVehicles();
        }

        private async void NavigateVehicleDetail(object sender, EventArgs args)
        {
            var vehicle = (Vehicle)((Button)sender).CommandParameter;

            var vehicleDetailPage = new VehicleDetail(vehicle);
            await Navigation.PushAsync(vehicleDetailPage, true);
        }

        private async void ShowNotes(object sender, TappedEventArgs e)
        {
            var vehicle = e.Parameter as Vehicle;

            var vehicleDataPage = new VehicleData(vehicle.Id, vehicle);
            await Navigation.PushAsync(vehicleDataPage, true);
        }
    }


}
