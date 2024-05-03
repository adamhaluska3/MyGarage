using SQLite;

namespace MauiApp1.Models;

public class Vehicle
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
    public VehicleType BodyType { get; set; }
    public FuelType FuelType { get; set; }

    public int ExpectedWeekOdo { get; set; }

    public Vehicle()
    {
    }

    public Vehicle(string name, string make, string model, int year, 
                   string regNumber, string vin, int color_r, int color_g, 
                   int color_b, VehicleType bodyType, FuelType fuelType, int expectedWeekOdo)
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
