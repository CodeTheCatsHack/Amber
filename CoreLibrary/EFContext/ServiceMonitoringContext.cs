using System.ComponentModel.DataAnnotations;
using CoreLibrary.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.EFContext;

public partial class ServiceMonitoringContext : DbContext
{
    public ServiceMonitoringContext(DbContextOptions<ServiceMonitoringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InformationUser> InformationUsers { get; set; }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<Marker> Markers { get; set; }

    public virtual DbSet<Satellite> Satellites { get; set; }

    public virtual DbSet<SatelliteImage> SatelliteImages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public IQueryable<string> SelectExpression(string nameFunction, List<string> parametrList)
    {
        return Database.SqlQueryRaw<string>(
            $"SELECT `{nameFunction}`({parametrList.Aggregate((x, y) => $"\"{x}\",\"{y}\"")});");
    }

    public IQueryable<string> SelectExpression(string nameFunction, string parametr)
    {
        return Database.SqlQueryRaw<string>($"SELECT `{nameFunction}`(\"{parametr}\");");
    }

    ///
    public string DecryptPassword([StringLength(255)] string encryptSoli)
    {
        return SelectExpression(nameof(DecryptPassword), encryptSoli).ToList()[0];
    }

    public string EncryptPassword([StringLength(255)] string soli)
    {
        return SelectExpression(nameof(EncryptPassword), soli).ToList()[0];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<InformationUser>(entity =>
        {
            entity.HasKey(e => e.IdInformationUser).HasName("PRIMARY");

            entity.Property(e => e.FirstName).HasComment("Имя пользователя.");
            entity.Property(e => e.LastName).HasComment("Фамилия пользователя.");
            entity.Property(e => e.MiddleName).HasComment("Отчество пользователя.");

            entity.HasOne(d => d.UserNavigation).WithOne(p => p.InformationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_InformationUser_User1");
        });

        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.IdMap).HasName("PRIMARY");

            entity.Property(e => e.IdMap).ValueGeneratedNever();

            entity.HasOne(d => d.InformationUserNavigation).WithOne(p => p.Map)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Map_InformationUser1");
        });

        modelBuilder.Entity<Marker>(entity =>
        {
            entity.HasKey(e => e.IdMarker).HasName("PRIMARY");

            entity.Property(e => e.IdMarker).ValueGeneratedNever();

            entity.HasOne(d => d.MapNavigation).WithMany(p => p.Markers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Marker_Map1");
        });

        modelBuilder.Entity<Satellite>(entity =>
        {
            entity.HasKey(e => e.IdSatellite).HasName("PRIMARY");

            entity.Property(e => e.IdSatellite).ValueGeneratedNever();

            entity.HasOne(d => d.MapNavigation).WithMany(p => p.Satellites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Satellite_Map1");
        });

        modelBuilder.Entity<SatelliteImage>(entity =>
        {
            entity.HasKey(e => e.IdSatelliteImage).HasName("PRIMARY");

            entity.HasOne(d => d.MarkerNavigation).WithMany(p => p.SatelliteImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_SatelliteImage_Marker1");

            entity.HasOne(d => d.SatelliteNavigation).WithMany(p => p.SatelliteImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_SatelliteImage_Satellite1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.Property(e => e.Login).HasComment("Логин пользователя.");
            entity.Property(e => e.Password).HasComment("Пароль пользователя.");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}