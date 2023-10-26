using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.EFModels;

[Table("User")]
[Index("Login", Name = "uq_login_password", IsUnique = true)]
public class User
{
    [Key] [Column("idUser")] public int IdUser { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [StringLength(100)]
    public string Login { get; set; } = null!;

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [StringLength(255)]
    public string Password { get; set; } = null!;

    public sbyte GroundStateAdministrator { get; set; }

    [InverseProperty("UserNavigation")] public virtual InformationUser? InformationUser { get; set; }
}