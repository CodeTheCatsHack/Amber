using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models.EFModels;

[Table("Satellite")]
public class Satellite
{
    [Key] [Column("idSatellite")] public int IdSatellite { get; set; }

    [StringLength(100)] public string SatName { get; set; } = null!;

    [Column(TypeName = "text")] public string TleLine1 { get; set; } = null!;

    [Column(TypeName = "text")] public string TleLine2 { get; set; } = null!;

    [Column(TypeName = "enum('Busy','NotBusy')")]
    public string Status { get; set; } = null!;

    public sbyte IsOffiicial { get; set; }

    [InverseProperty("SatelliteNavigation")]
    public virtual ICollection<SatelliteImage> SatelliteImages { get; set; } = new List<SatelliteImage>();

    [ForeignKey("Satellite")]
    [InverseProperty("Satellites")]
    public virtual ICollection<Map> Maps { get; set; } = new List<Map>();
}