using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MyGarage.Models;

public class Vehicle : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } = -1;

    [MaxLength(Constants.VehicleNameLength)]
    public string Name { get; set; }
    [MaxLength(Constants.VehicleMakeModelLength)]
    public string Make { get; set; }
    [MaxLength(Constants.VehicleMakeModelLength)]
    public string Model { get; set; }
    public int Year { get; set; }

    [MaxLength(Constants.VehicleRegNumLength), Unique]
    public string RegNumber { get; set; }
    [MaxLength(Constants.VehicleVinLength), Unique]
    public string VIN {  get; set; }

    public VehicleType BodyType { get; set; }
    public string ImageSource {  get; set; }
    public FuelType FuelType { get; set; }

    public int ExpectedWeekOdo { get; set; }

    public Vehicle()
    {
    }

    public Vehicle(string name, string make, string model, int year, 
                   string regNumber, string vin, VehicleType bodyType, 
                   FuelType fuelType, int expectedWeekOdo)
    {
        Name = name;
        Make = make;
        Model = model;
        Year = year;
        RegNumber = regNumber;
        VIN = vin;
        BodyType = bodyType;
        FuelType = fuelType;
        ExpectedWeekOdo = expectedWeekOdo;
    }
}
