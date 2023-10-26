﻿// <auto-generated />
using System;
using CoreLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoreLibrary.Models.Migrated
{
    [DbContext(typeof(QuartzDbContext))]
    partial class QuartzDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzBlobTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<byte[]>("BlobData")
                        .HasColumnType("blob")
                        .HasColumnName("BLOB_DATA");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_BLOB_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzCalendar", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("CalendarName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("CALENDAR_NAME");

                    b.Property<byte[]>("Calendar")
                        .IsRequired()
                        .HasColumnType("blob")
                        .HasColumnName("CALENDAR");

                    b.HasKey("SchedulerName", "CalendarName");

                    b.ToTable("QRTZ_CALENDARS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzCronTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<string>("CronExpression")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasColumnName("CRON_EXPRESSION");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("varchar(80)")
                        .HasColumnName("TIME_ZONE_ID");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_CRON_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzFiredTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("EntryId")
                        .HasColumnType("varchar(140)")
                        .HasColumnName("ENTRY_ID");

                    b.Property<long>("FiredTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("FIRED_TIME");

                    b.Property<string>("InstanceName")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("INSTANCE_NAME");

                    b.Property<bool>("IsNonConcurrent")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("IS_NONCONCURRENT");

                    b.Property<string>("JobGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("JOB_GROUP");

                    b.Property<string>("JobName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("JOB_NAME");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("PRIORITY");

                    b.Property<bool?>("RequestsRecovery")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("REQUESTS_RECOVERY");

                    b.Property<long>("ScheduledTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("SCHED_TIME");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasColumnName("STATE");

                    b.Property<string>("TriggerGroup")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<string>("TriggerName")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_NAME");

                    b.HasKey("SchedulerName", "EntryId");

                    b.HasIndex("InstanceName")
                        .HasDatabaseName("IDX_QRTZ_FT_TRIG_INST_NAME");

                    b.HasIndex("JobGroup")
                        .HasDatabaseName("IDX_QRTZ_FT_JOB_GROUP");

                    b.HasIndex("JobName")
                        .HasDatabaseName("IDX_QRTZ_FT_JOB_NAME");

                    b.HasIndex("RequestsRecovery")
                        .HasDatabaseName("IDX_QRTZ_FT_JOB_REQ_RECOVERY");

                    b.HasIndex("TriggerGroup")
                        .HasDatabaseName("IDX_QRTZ_FT_TRIG_GROUP");

                    b.HasIndex("TriggerName")
                        .HasDatabaseName("IDX_QRTZ_FT_TRIG_NAME");

                    b.HasIndex("SchedulerName", "TriggerName", "TriggerGroup")
                        .HasDatabaseName("IDX_QRTZ_FT_TRIG_NM_GP");

                    b.ToTable("QRTZ_FIRED_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzJobDetail", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("JobName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("JOB_NAME");

                    b.Property<string>("JobGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("JOB_GROUP");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<bool>("IsDurable")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("IS_DURABLE");

                    b.Property<bool>("IsNonConcurrent")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("IS_NONCONCURRENT");

                    b.Property<bool>("IsUpdateData")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("IS_UPDATE_DATA");

                    b.Property<string>("JobClassName")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("JOB_CLASS_NAME");

                    b.Property<byte[]>("JobData")
                        .HasColumnType("blob")
                        .HasColumnName("JOB_DATA");

                    b.Property<bool>("RequestsRecovery")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("REQUESTS_RECOVERY");

                    b.HasKey("SchedulerName", "JobName", "JobGroup");

                    b.HasIndex("RequestsRecovery")
                        .HasDatabaseName("IDX_QRTZ_J_REQ_RECOVERY");

                    b.ToTable("QRTZ_JOB_DETAILS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzLock", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("LockName")
                        .HasColumnType("varchar(40)")
                        .HasColumnName("LOCK_NAME");

                    b.HasKey("SchedulerName", "LockName");

                    b.ToTable("QRTZ_LOCKS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzPausedTriggerGroup", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.HasKey("SchedulerName", "TriggerGroup");

                    b.ToTable("QRTZ_PAUSED_TRIGGER_GRPS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSchedulerState", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("InstanceName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("INSTANCE_NAME");

                    b.Property<long>("CheckInInterval")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("CHECKIN_INTERVAL");

                    b.Property<long>("LastCheckInTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("LAST_CHECKIN_TIME");

                    b.HasKey("SchedulerName", "InstanceName");

                    b.ToTable("QRTZ_SCHEDULER_STATE", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimplePropertyTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<bool?>("BooleanProperty1")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("BOOL_PROP_1");

                    b.Property<bool?>("BooleanProperty2")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("BOOL_PROP_2");

                    b.Property<decimal?>("DecimalProperty1")
                        .HasColumnType("NUMERIC(13,4)")
                        .HasColumnName("DEC_PROP_1");

                    b.Property<decimal?>("DecimalProperty2")
                        .HasColumnType("NUMERIC(13,4)")
                        .HasColumnName("DEC_PROP_2");

                    b.Property<int?>("IntegerProperty1")
                        .HasColumnType("int")
                        .HasColumnName("INT_PROP_1");

                    b.Property<int?>("IntegerProperty2")
                        .HasColumnType("int")
                        .HasColumnName("INT_PROP_2");

                    b.Property<long?>("LongProperty1")
                        .HasColumnType("BIGINT")
                        .HasColumnName("LONG_PROP_1");

                    b.Property<long?>("LongProperty2")
                        .HasColumnType("BIGINT")
                        .HasColumnName("LONG_PROP_2");

                    b.Property<string>("StringProperty1")
                        .HasColumnType("varchar(512)")
                        .HasColumnName("STR_PROP_1");

                    b.Property<string>("StringProperty2")
                        .HasColumnType("varchar(512)")
                        .HasColumnName("STR_PROP_2");

                    b.Property<string>("StringProperty3")
                        .HasColumnType("varchar(512)")
                        .HasColumnName("STR_PROP_3");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("varchar(80)")
                        .HasColumnName("TIME_ZONE_ID");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_SIMPROP_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimpleTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<long>("RepeatCount")
                        .HasColumnType("bigint(7)")
                        .HasColumnName("REPEAT_COUNT");

                    b.Property<long>("RepeatInterval")
                        .HasColumnType("bigint(12)")
                        .HasColumnName("REPEAT_INTERVAL");

                    b.Property<long>("TimesTriggered")
                        .HasColumnType("bigint(10)")
                        .HasColumnName("TIMES_TRIGGERED");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_SIMPLE_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("varchar(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<string>("CalendarName")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("CALENDAR_NAME");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<long?>("EndTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("END_TIME");

                    b.Property<byte[]>("JobData")
                        .HasColumnType("blob")
                        .HasColumnName("JOB_DATA");

                    b.Property<string>("JobGroup")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("JOB_GROUP");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("JOB_NAME");

                    b.Property<short?>("MisfireInstruction")
                        .HasColumnType("smallint(2)")
                        .HasColumnName("MISFIRE_INSTR");

                    b.Property<long?>("NextFireTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("NEXT_FIRE_TIME");

                    b.Property<long?>("PreviousFireTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("PREV_FIRE_TIME");

                    b.Property<int?>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("PRIORITY");

                    b.Property<long>("StartTime")
                        .HasColumnType("bigint(19)")
                        .HasColumnName("START_TIME");

                    b.Property<string>("TriggerState")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasColumnName("TRIGGER_STATE");

                    b.Property<string>("TriggerType")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("TRIGGER_TYPE");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.HasIndex("NextFireTime")
                        .HasDatabaseName("IDX_QRTZ_T_NEXT_FIRE_TIME");

                    b.HasIndex("TriggerState")
                        .HasDatabaseName("IDX_QRTZ_T_STATE");

                    b.HasIndex("NextFireTime", "TriggerState")
                        .HasDatabaseName("IDX_QRTZ_T_NFT_ST");

                    b.HasIndex("SchedulerName", "JobName", "JobGroup");

                    b.ToTable("QRTZ_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzBlobTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("BlobTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzCronTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("CronTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimplePropertyTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("SimplePropertyTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimpleTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("SimpleTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzJobDetail", "JobDetail")
                        .WithMany("Triggers")
                        .HasForeignKey("SchedulerName", "JobName", "JobGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobDetail");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzJobDetail", b =>
                {
                    b.Navigation("Triggers");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", b =>
                {
                    b.Navigation("BlobTriggers");

                    b.Navigation("CronTriggers");

                    b.Navigation("SimplePropertyTriggers");

                    b.Navigation("SimpleTriggers");
                });
#pragma warning restore 612, 618
        }
    }
}
