using MyGarage.Models;

namespace MyGarage.Views;

public partial class VehicleData : TabbedPage
{
    public VehicleData(int vehicleId, Vehicle vehicle)
    {
        InitializeComponent();

        Children.Add(new NoteList(vehicleId, vehicle));
        Children.Add(new OdometerData(vehicleId));
    }
}