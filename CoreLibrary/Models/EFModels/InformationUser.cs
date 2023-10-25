using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[PrimaryKey("IdInformationUser", "UserId")]
[Table("InformationUser")]
[Index("UserId", Name = "fk_InformationUser_User_idx", IsUnique = true)]
public class InformationUser
{
    [Key] [Column("idInformationUser")] public int IdInformationUser { get; set; }

    /// <summary>
    ///     Идентификатор связанного пользователя.
    /// </summary>
    [Key]
    public int UserId { get; set; }

    /// <summary>
    ///     Имя пользователя.
    /// </summary>
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    ///     Фамилия пользователя.
    /// </summary>
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    /// <summary>
    ///     Отчество пользователя.
    /// </summary>
    [StringLength(50)]
    public string? MiddleName { get; set; }

    [Column(TypeName = "text")]
    public string Avatar { get; set; } = "https://pixelbox.ru/wp-content/uploads/2021/09/avatar-boys-vk-46.jpg";

    [InverseProperty("InformationUserNavigation")]
    public virtual ICollection<Map> Maps { get; set; } = new List<Map>();

    [ForeignKey("UserId")]
    [JsonIgnore]
    [InverseProperty("InformationUser")]
    public virtual User User { get; set; } = null!;
}