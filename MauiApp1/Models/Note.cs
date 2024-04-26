using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MauiApp1.Models;


class Note
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ForeignKey("Id")]
    public int VehicleId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }

    public DateTime CreationTime { get; set; }
    public int OdoRemind { get; set; }

    public Note()
    {
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
