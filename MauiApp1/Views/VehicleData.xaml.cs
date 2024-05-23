using MyGarage.Models;

namespace MyGarage.Views;

public partial class VehicleData : TabbedPage
{
	private int vehicleId;
	private Vehicle currentVehicle;
	public VehicleData(int vehicleId, Vehicle vehicle)
	{
		this.vehicleId = vehicleId;
        currentVehicle = vehicle;

		InitializeComponent();

		Children.Add(new NoteList(vehicleId, currentVehicle));
		Children.Add(new OdometerData(vehicleId));
	}
}