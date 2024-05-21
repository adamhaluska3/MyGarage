using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGarage.Models.VisualModels;

internal class VehicleItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Vehicle Vehicle { get; set; }

    public VehicleItem(string name, string description, Vehicle vehicle)
    {
        Name = name;
        Description = description;
        Vehicle = vehicle;
    }
}
