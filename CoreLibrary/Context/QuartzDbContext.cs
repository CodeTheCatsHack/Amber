using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.MySql;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Context;

public class QuartzDbContext : DbContext
{
    public QuartzDbContext(DbContextOptions<QuartzDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddQuartz(x => x.UseMySql());

        base.OnModelCreating(modelBuilder);
    }
}