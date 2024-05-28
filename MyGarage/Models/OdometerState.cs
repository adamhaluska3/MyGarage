using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyGarage.Models;

public class OdometerState : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } = -1;

    [ForeignKey("Id")]
    public int VehicleId { get; set; }

    public DateTime DateTime { get; set; }
    public int State { get; set; }

    public OdometerState()
    {
    }

    public OdometerState(int vehicleId)
    {
        VehicleId = vehicleId;
    }
}
