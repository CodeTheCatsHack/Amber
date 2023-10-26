using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("Satellite")]
[Index("Map", Name = "fk_Satellite_Map1_idx")]
public class Satellite
{
    [Key] [Column("idSatellite")] public int IdSatellite { get; set; }

    [StringLength(100)] public string SatName { get; set; } = null!;

    [Column(TypeName = "text")] public string TleLine1 { get; set; } = null!;

    [Column(TypeName = "text")] public string TleLine2 { get; set; } = null!;

    [Column(TypeName = "enum('Busy','NotBusy')")]
    public string Status { get; set; } = null!;

    public sbyte IsOffiicial { get; set; }

    public int Map { get; set; }

    [ForeignKey("Map")]
    [InverseProperty("Satellites")]
    public virtual Map MapNavigation { get; set; } = null!;

    [InverseProperty("SatelliteNavigation")]
    public virtual ICollection<SatelliteImage> SatelliteImages { get; set; } = new List<SatelliteImage>();
}