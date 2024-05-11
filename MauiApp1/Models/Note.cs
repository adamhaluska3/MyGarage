using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MauiApp1.Models;


public class Note : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } = -1;

    [ForeignKey("Id")]
    public int VehicleId { get; set; }

    public int Type { get; set; } // NoteType
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

    public Note(int id, int vehicleId, string name, string description, DateTime creationTime, int odoRemind)
    {
        Id = id;
        VehicleId = vehicleId;
        Name = name;
        Description = description;
        CreationTime = creationTime;
        OdoRemind = odoRemind;
    }
}
