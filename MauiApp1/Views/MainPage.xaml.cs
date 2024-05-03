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
                items.Add(new VehicleItem(vehicle.Name, $"{vehicle.Make} {vehicle.Model}", vehicle));
            }

            VehiclesListView.ItemsSource = items;
        }

        private async void CreateVehicle(object sender, EventArgs args)
        {
            var vehicle = new Vehicle();
            var vehicleDetailPage = new VehicleDetail(vehicle);
            // TODO
            try
            {
                vehicleDetailPage.BindingContext = vehicle;
            }
            catch { }
            await Navigation.PushAsync(vehicleDetailPage, true);
        }
        private async void Reload(object sender, EventArgs args)
        {
            await LoadVehicles();
        }

        private async void NavigateVehicleDetail(object sender, TappedEventArgs args)
        {
            var vehicle = args.Parameter as Vehicle;
            var vehicleDetailPage = new VehicleDetail(vehicle);
            // TODO
            try
            {
                vehicleDetailPage.BindingContext = vehicle;
            }
            catch { }
            await Navigation.PushAsync(vehicleDetailPage, true);
        }


        protected override async void OnAppearing()
        {
            await LoadVehicles();
            base.OnAppearing();
        }
    }


}
