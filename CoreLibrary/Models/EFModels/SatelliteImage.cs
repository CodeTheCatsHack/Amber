using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("SatelliteImage")]
[Index("Marker", Name = "fk_SatelliteImage_Marker1_idx")]
[Index("Satellite", Name = "fk_SatelliteImage_Satellite1_idx")]
public class SatelliteImage
{
    [Key] [Column("idSatelliteImage")] public int IdSatelliteImage { get; set; }

    [Column(TypeName = "blob")] public byte[] Image { get; set; } = null!;

    public int Satellite { get; set; }

    public int Marker { get; set; }

    [ForeignKey("Marker")]
    [InverseProperty("SatelliteImages")]
    public virtual Marker MarkerNavigation { get; set; } = null!;

    [ForeignKey("Satellite")]
    [InverseProperty("SatelliteImages")]
    public virtual Satellite SatelliteNavigation { get; set; } = null!;
}