using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLite;

namespace MauiApp1.Models.Viewmodels;

[QueryProperty("vehicle", "vehicle")]
public partial class VehicleDetailViewModel : ObservableObject, IQueryAttributable
{
    public int Id {  get; set; }
    public string Name { get; private set; }
    public string Make { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }

    public string RegNumber { get; private set; }
    public string VIN { get; private set; }

    public int Color_R { get; private set; }
    public int Color_G { get; private set; }
    public int Color_B { get; private set; }
    public VehicleType BodyType { get; private set; }
    public FuelType FuelType { get; private set; }

    public int ExpectedWeekOdo { get; private set; }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var vehicle = (Vehicle)query["vehicle"];

        Id = vehicle.Id;

        Name = vehicle.Name;
        Make = vehicle.Make;
        Model = vehicle.Model;
        Year = vehicle.Year;

        RegNumber = vehicle.RegNumber;
        VIN = vehicle.VIN;

        Color_R = vehicle.Color_R;
        Color_G = vehicle.Color_G;
        Color_B = vehicle.Color_B;
        BodyType = vehicle.BodyType;
        FuelType = vehicle.FuelType;

        ExpectedWeekOdo = vehicle.ExpectedWeekOdo;
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

}
