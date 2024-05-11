using MauiApp1.Models;
using MauiApp1.Models.VisualModels;
using MauiApp1.Views;

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

            VehiclesCollectionView.ItemsSource = items;
        }

        private async void CreateVehicle(object sender, EventArgs args)
        {
            var vehicle = new Vehicle();
            var vehicleDetailPage = new VehicleDetail(vehicle);
            vehicleDetailPage.BindingContext = vehicle;

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


        protected override async void OnAppearing()
        {
            await LoadVehicles();
            base.OnAppearing();
        }

        private async void ShowNotes(object sender, TappedEventArgs e)
        {
            var vehicle = e.Parameter as Vehicle;

            var noteListpage = new NoteList(vehicle.Id);

            var currentVehicle = await App.Database.GetVehicle(vehicle.Id);
            noteListpage.BindingContext = currentVehicle;

            await Navigation.PushAsync(noteListpage, true);
        }
    }


}
