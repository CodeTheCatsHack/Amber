using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[PrimaryKey("Satellite", "Map")]
[Table("MapListSatellite")]
[Index("Map", Name = "fk_Satellite_has_Map_Map1_idx")]
[Index("Satellite", Name = "fk_Satellite_has_Map_Satellite1_idx")]
public class MapListSatellite
{
    [Key] public int Satellite { get; set; }

    [Key] public int Map { get; set; }

    [ForeignKey("Satellite")]
    [InverseProperty("MapListSatellites")]
    public virtual Satellite SatelliteNavigation { get; set; } = null!;
}