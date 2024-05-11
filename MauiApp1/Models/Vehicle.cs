using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MyGarage.Models;

public class Vehicle : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } = -1;

    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(50)]
    public string Make { get; set; }
    [MaxLength(50)]
    public string Model { get; set; }
    public int Year { get; set; }

    [MaxLength(20), Unique]
    public string RegNumber { get; set; }
    [MaxLength(50), Unique]
    public string VIN {  get; set; }

    public int Color_R { get; set; }
    public int Color_G { get; set; }
    public int Color_B { get; set; }
    public int BodyType { get; set; } // VehicleType
    public string ImageSource {  get; set; }
    public int FuelType { get; set; } // FuelType

    public int ExpectedWeekOdo { get; set; }

    public Vehicle()
    {
    }

    public Vehicle(string name, string make, string model, int year, 
                   string regNumber, string vin, int color_r, int color_g, 
                   int color_b, int bodyType, int fuelType, int expectedWeekOdo)
    {
        Name = name;
        Make = make;
        Model = model;
        Year = year;
        RegNumber = regNumber;
        VIN = vin;
        Color_R = color_r;
        Color_G = color_g;
        Color_B = color_b;
        BodyType = bodyType;
        FuelType = fuelType;
        ExpectedWeekOdo = expectedWeekOdo;
    }
}
