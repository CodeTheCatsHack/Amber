﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("Marker")]
[Index("Map", Name = "fk_Marker_Map1_idx")]
public class Marker
{
    [Key] [Column("idMarker")] public int IdMarker { get; set; }

    [Column(TypeName = "text")] public string Name { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int Map { get; set; } = 1;

    public double Radius { get; set; }

    [ForeignKey("Map")]
    [InverseProperty("Markers")]
    public virtual Map MapNavigation { get; set; }

    [InverseProperty("MarkerNavigation")]
    public virtual ICollection<SatelliteImage> SatelliteImages { get; set; } = new List<SatelliteImage>();
}