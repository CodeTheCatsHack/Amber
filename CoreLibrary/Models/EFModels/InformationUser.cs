using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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

    [Column(TypeName = "text")]
    public string Avatar { get; set; } =
        "https://w-dog.ru/wallpapers/16/10/374986844638692/iskusstvo-misaki-kurehito-devushka-paren-vzglyad-nastroenie-ulybka-kniga-cvety-nebo-boku-v-kanojo-ni-furu-yoru.jpg";

    public int User { get; set; }

    [InverseProperty("InformationUserNavigation")]
    public virtual Map? Map { get; set; }

    [JsonIgnore]
    [ForeignKey("User")]
    [InverseProperty("InformationUser")]
    public virtual User UserNavigation { get; set; } = null!;
}