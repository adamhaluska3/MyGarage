using MauiApp1.Models;
using MauiApp1.Models.Viewmodels;

namespace MauiApp1;

public partial class VehicleDetail : ContentPage
{

	public VehicleDetail(VehicleDetailViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
	}
}