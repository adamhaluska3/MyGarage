﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.VisualModels;

internal class VehicleItem
{
    public string Name { get; set; }
    public string Description { get; set; }

    public VehicleItem(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
