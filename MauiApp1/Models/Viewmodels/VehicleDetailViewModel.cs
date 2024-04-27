using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.Models.Viewmodels;

public partial class VehicleDetailViewModel : ObservableObject, IQueryAttributable
{
    public Vehicle vehicle {  get; private set; }
    public string Name { get; private set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        vehicle = (Vehicle)query["vehicle"];
        Name = vehicle.Name;
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

}
