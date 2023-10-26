using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[PrimaryKey("IdShootingАrea", "MarkerMap")]
[Table("ShootingАrea")]
[Index("MarkerMap", Name = "fk_ShootingАrea_Marker1_idx")]
[Index("SatelliteImage", Name = "fk_ShootingАrea_SatelliteImage1_idx")]
public class ShootingАrea
{
    [Key] [Column("idShootingАrea")] public int IdShootingАrea { get; set; }

    [Column(TypeName = "text")] public string NameArea { get; set; } = null!;

    public double Radius { get; set; }

    [Key] public int MarkerMap { get; set; }

    public int SatelliteImage { get; set; }

    [JsonIgnore]
    [ForeignKey("MarkerMap")]
    [InverseProperty("ShootingАreas")]
    public virtual Marker MarkerMapNavigation { get; set; } = null!;

    [ForeignKey("SatelliteImage")]
    [InverseProperty("ShootingАreas")]
    public virtual SatelliteImage SatelliteImageNavigation { get; set; } = null!;
}