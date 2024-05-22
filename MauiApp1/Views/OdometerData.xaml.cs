namespace MyGarage.Views;

public partial class OdometerData : ContentPage
{
	private int vehicleId;
	public OdometerData(int vehicleId)
	{
		this.vehicleId = vehicleId;

		InitializeComponent();
	}
}