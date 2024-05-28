using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyGarage.Models;

public class Note : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } = -1;

    [ForeignKey("Id")]
    public int VehicleId { get; set; }

    public NoteType Type { get; set; }
    public string ImageSource { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }

    public DateTime CreationTime { get; set; }

    public bool HasRemind { get; set; }
    public int OdoRemind { get; set; }

    public Note()
    {
    }

    public Note(int vehicleId)
    {
        VehicleId = vehicleId;
    }
}
