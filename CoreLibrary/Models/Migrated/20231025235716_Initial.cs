#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLibrary.Models.Migrated;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_CALENDARS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CALENDAR_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CALENDAR = table.Column<byte[]>("blob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CALENDARS", x => new { x.SCHED_NAME, x.CALENDAR_NAME });
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_FIRED_TRIGGERS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ENTRY_ID = table.Column<string>("varchar(140)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INSTANCE_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FIRED_TIME = table.Column<long>("bigint(19)", nullable: false),
                    SCHED_TIME = table.Column<long>("bigint(19)", nullable: false),
                    PRIORITY = table.Column<int>("integer", nullable: false),
                    STATE = table.Column<string>("varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>("varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>("varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_NONCONCURRENT = table.Column<bool>("tinyint(1)", nullable: false),
                    REQUESTS_RECOVERY = table.Column<bool>("tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_FIRED_TRIGGERS", x => new { x.SCHED_NAME, x.ENTRY_ID });
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_JOB_DETAILS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>("varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_CLASS_NAME = table.Column<string>("varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_DURABLE = table.Column<bool>("tinyint(1)", nullable: false),
                    IS_NONCONCURRENT = table.Column<bool>("tinyint(1)", nullable: false),
                    IS_UPDATE_DATA = table.Column<bool>("tinyint(1)", nullable: false),
                    REQUESTS_RECOVERY = table.Column<bool>("tinyint(1)", nullable: false),
                    JOB_DATA = table.Column<byte[]>("blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_JOB_DETAILS", x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP });
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_LOCKS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LOCK_NAME = table.Column<string>("varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table => { table.PrimaryKey("PK_QRTZ_LOCKS", x => new { x.SCHED_NAME, x.LOCK_NAME }); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_PAUSED_TRIGGER_GRPS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_PAUSED_TRIGGER_GRPS", x => new { x.SCHED_NAME, x.TRIGGER_GROUP });
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_SCHEDULER_STATE",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INSTANCE_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LAST_CHECKIN_TIME = table.Column<long>("bigint(19)", nullable: false),
                    CHECKIN_INTERVAL = table.Column<long>("bigint(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SCHEDULER_STATE", x => new { x.SCHED_NAME, x.INSTANCE_NAME });
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_TRIGGERS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>("varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NEXT_FIRE_TIME = table.Column<long>("bigint(19)", nullable: true),
                    PREV_FIRE_TIME = table.Column<long>("bigint(19)", nullable: true),
                    PRIORITY = table.Column<int>("integer", nullable: true),
                    TRIGGER_STATE = table.Column<string>("varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_TYPE = table.Column<string>("varchar(8)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    START_TIME = table.Column<long>("bigint(19)", nullable: false),
                    END_TIME = table.Column<long>("bigint(19)", nullable: true),
                    CALENDAR_NAME = table.Column<string>("varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MISFIRE_INSTR = table.Column<short>("smallint(2)", nullable: true),
                    JOB_DATA = table.Column<byte[]>("blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        "FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS_SCHED_NAME_JOB_NAME_JOB_GROUP",
                        x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP },
                        "QRTZ_JOB_DETAILS",
                        new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_BLOB_TRIGGERS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BLOB_DATA = table.Column<byte[]>("blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_BLOB_TRIGGERS",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        "FK_QRTZ_BLOB_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRI~",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        "QRTZ_TRIGGERS",
                        new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_CRON_TRIGGERS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CRON_EXPRESSION = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIME_ZONE_ID = table.Column<string>("varchar(80)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CRON_TRIGGERS",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        "FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRI~",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        "QRTZ_TRIGGERS",
                        new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_SIMPLE_TRIGGERS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    REPEAT_COUNT = table.Column<long>("bigint(7)", nullable: false),
                    REPEAT_INTERVAL = table.Column<long>("bigint(12)", nullable: false),
                    TIMES_TRIGGERED = table.Column<long>("bigint(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPLE_TRIGGERS",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        "FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_T~",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        "QRTZ_TRIGGERS",
                        new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "QRTZ_SIMPROP_TRIGGERS",
                table => new
                {
                    SCHED_NAME = table.Column<string>("varchar(120)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>("varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_1 = table.Column<string>("varchar(512)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_2 = table.Column<string>("varchar(512)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_3 = table.Column<string>("varchar(512)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INT_PROP_1 = table.Column<int>("int", nullable: true),
                    INT_PROP_2 = table.Column<int>("int", nullable: true),
                    LONG_PROP_1 = table.Column<long>("BIGINT", nullable: true),
                    LONG_PROP_2 = table.Column<long>("BIGINT", nullable: true),
                    DEC_PROP_1 = table.Column<decimal>("NUMERIC(13,4)", nullable: true),
                    DEC_PROP_2 = table.Column<decimal>("NUMERIC(13,4)", nullable: true),
                    BOOL_PROP_1 = table.Column<bool>("tinyint(1)", nullable: true),
                    BOOL_PROP_2 = table.Column<bool>("tinyint(1)", nullable: true),
                    TIME_ZONE_ID = table.Column<string>("varchar(80)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPROP_TRIGGERS",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        "FK_QRTZ_SIMPROP_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_~",
                        x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        "QRTZ_TRIGGERS",
                        new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_JOB_GROUP",
            "QRTZ_FIRED_TRIGGERS",
            "JOB_GROUP");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_JOB_NAME",
            "QRTZ_FIRED_TRIGGERS",
            "JOB_NAME");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_JOB_REQ_RECOVERY",
            "QRTZ_FIRED_TRIGGERS",
            "REQUESTS_RECOVERY");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_TRIG_GROUP",
            "QRTZ_FIRED_TRIGGERS",
            "TRIGGER_GROUP");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_TRIG_INST_NAME",
            "QRTZ_FIRED_TRIGGERS",
            "INSTANCE_NAME");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_TRIG_NAME",
            "QRTZ_FIRED_TRIGGERS",
            "TRIGGER_NAME");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_FT_TRIG_NM_GP",
            "QRTZ_FIRED_TRIGGERS",
            new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_J_REQ_RECOVERY",
            "QRTZ_JOB_DETAILS",
            "REQUESTS_RECOVERY");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_T_NEXT_FIRE_TIME",
            "QRTZ_TRIGGERS",
            "NEXT_FIRE_TIME");

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_T_NFT_ST",
            "QRTZ_TRIGGERS",
            new[] { "NEXT_FIRE_TIME", "TRIGGER_STATE" });

        migrationBuilder.CreateIndex(
            "IDX_QRTZ_T_STATE",
            "QRTZ_TRIGGERS",
            "TRIGGER_STATE");

        migrationBuilder.CreateIndex(
            "IX_QRTZ_TRIGGERS_SCHED_NAME_JOB_NAME_JOB_GROUP",
            "QRTZ_TRIGGERS",
            new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "QRTZ_BLOB_TRIGGERS");

        migrationBuilder.DropTable(
            "QRTZ_CALENDARS");

        migrationBuilder.DropTable(
            "QRTZ_CRON_TRIGGERS");

        migrationBuilder.DropTable(
            "QRTZ_FIRED_TRIGGERS");

        migrationBuilder.DropTable(
            "QRTZ_LOCKS");

        migrationBuilder.DropTable(
            "QRTZ_PAUSED_TRIGGER_GRPS");

        migrationBuilder.DropTable(
            "QRTZ_SCHEDULER_STATE");

        migrationBuilder.DropTable(
            "QRTZ_SIMPLE_TRIGGERS");

        migrationBuilder.DropTable(
            "QRTZ_SIMPROP_TRIGGERS");

        migrationBuilder.DropTable(
            "QRTZ_TRIGGERS");

        migrationBuilder.DropTable(
            "QRTZ_JOB_DETAILS");
    }
}