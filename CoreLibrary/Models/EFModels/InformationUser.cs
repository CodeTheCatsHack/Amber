using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("InformationUser")]
[Index("User", Name = "fk_InformationUser_User1_idx", IsUnique = true)]
public class InformationUser
{
    [Key] [Column("idInformationUser")] public int IdInformationUser { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Отчество пользователя.
    /// </summary>
    [StringLength(50)]
    public string? MiddleName { get; set; }

    [Column(TypeName = "text")] public string Avatar { get; set; } = null!;

    public int User { get; set; }

    [InverseProperty("InformationUserNavigation")]
    public virtual Map? Map { get; set; }

    [ForeignKey("User")]
    [InverseProperty("InformationUser")]
    public virtual User UserNavigation { get; set; } = null!;
}