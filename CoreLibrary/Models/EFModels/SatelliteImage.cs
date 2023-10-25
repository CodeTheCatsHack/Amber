using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("SatelliteImage")]
[Index("Satellite", Name = "fk_SatelliteImage_Satellite1_idx")]
public class SatelliteImage
{
    [Key] [Column("idSatelliteImage")] public int IdSatelliteImage { get; set; }

    [Column(TypeName = "blob")] public byte[] Image { get; set; } = null!;

    public int Satellite { get; set; }

    [ForeignKey("Satellite")]
    [InverseProperty("SatelliteImages")]
    public virtual Satellite SatelliteNavigation { get; set; } = null!;

    [InverseProperty("SatelliteImageNavigation")]
    public virtual ICollection<ShootingАrea> ShootingАreas { get; set; } = new List<ShootingАrea>();
}