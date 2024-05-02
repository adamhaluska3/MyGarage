﻿using MauiApp1.Models;
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

        private async void AddVehicle(object sender, EventArgs args)
        {
            var vehicle = new Vehicle("Fabija", "Skoda", "Fabia", 2009, "MI1821222FO", "tmm12db", 0, 0, 0, VehicleType.MPV, FuelType.LPG, 600);
            await App.Database.AddNewVehicle(vehicle);
            await LoadVehicles();
        }
        private async void Reload(object sender, EventArgs args)
        {
            await LoadVehicles();
        }

        private async void NavigateVehicleDetail(object sender, TappedEventArgs args)
        {
            var vehicle = args.Parameter as Vehicle;
            var vehicleDetailPage = new VehicleDetail(vehicle);
            vehicleDetailPage.BindingContext = vehicle;
            await Navigation.PushAsync(vehicleDetailPage, true);
        }


        protected override async void OnAppearing()
        {
            await LoadVehicles();
            base.OnAppearing();
        }
    }


}