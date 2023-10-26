using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("Map")]
[Index("InformationUser", Name = "fk_Map_InformationUser1_idx", IsUnique = true)]
public class Map
{
    [Key] [Column("idMap")] public int IdMap { get; set; }

    [StringLength(100)] public string Name { get; set; } = null!;

    [Column(TypeName = "text")] public string? Description { get; set; }

    public int User { get; set; }

    public int InformationUser { get; set; }

    [ForeignKey("InformationUser")]
    [InverseProperty("Map")]
    public virtual InformationUser InformationUserNavigation { get; set; } = null!;

    [InverseProperty("MapNavigation")] public virtual ICollection<Marker> Markers { get; set; } = new List<Marker>();

    [InverseProperty("MapNavigation")]
    public virtual ICollection<Satellite> Satellites { get; set; } = new List<Satellite>();
}