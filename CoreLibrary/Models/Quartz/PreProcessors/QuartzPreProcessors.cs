using Quartz;
using Quartz.Impl.Calendar;

namespace CoreLibrary.Models.Quartz.PreProcessors;

public static class QuartzPreProcessors
{
    public static void AddCallendarsQuartz(this IServiceCollectionQuartzConfigurator configurator,
        params TaskExcludedCallendar[] calendars)
    {
        foreach (var taskCallendar in calendars)
            configurator.AddCalendar<HolidayCalendar>(
                taskCallendar.CallendarName,
                taskCallendar.Replace,
                taskCallendar.UpdateTriggers,
                x => x.AddExcludedDate(taskCallendar.ExcludedDate)
            );
    }

    public static void AddJobsQuartz(this IServiceCollectionQuartzConfigurator configurator,
        params TaskDetailJob[] taskDetailJobs)
    {
        foreach (var taskJob in taskDetailJobs)
            configurator.AddJob(taskJob.JobType, taskJob.JobIdentity, jobConfigurator => jobConfigurator
                    .WithDescription(taskJob.Description)
                    .PersistJobDataAfterExecution(taskJob
                        .JobAfterInDataHistory) //Сохранять ли информацию о задаче после выполнения. (true)
                    .StoreDurably(taskJob
                        .JobDeleteIfNotTrigger) //Должно ли задание оставаться в хранилище, если на него не указывает не один триггер. (true)
                    .DisallowConcurrentExecution(taskJob
                        .NotConcurrentExecute) //Указывает, должно ли быть запрещено одновременное выполнения задания. (true)
            );
    }

    public static void AddTriggersQuartz(this IServiceCollectionQuartzConfigurator configurator,
        params TaskDetailTrigger[] taskDetailTriggers)
    {
        foreach (var taskTrigger in taskDetailTriggers)
            configurator.AddTrigger(triggerConfigurator =>
            {
                triggerConfigurator.WithIdentity(taskTrigger.NameTrigger)
                    .WithDescription(taskTrigger.Description)
                    .ForJob(taskTrigger.NameJob) //Привязка к заданию.
                    .StartNow() //Запуск триггера в зависимости от расписания
                    .StartAt(taskTrigger
                        .StartTime); // Запуск триггера в зависимости от расписания не раньше указанного времени.

                if (taskTrigger.SimpleInterval is not null)
                    triggerConfigurator.WithSimpleSchedule(taskTrigger
                        .SimpleInterval); // Время между запусками задачи на интервалах и опциях.

                if (taskTrigger.CallendarInterval is not null)
                    triggerConfigurator.WithCalendarIntervalSchedule(taskTrigger.CallendarInterval)
                        .ModifiedByCalendar(taskTrigger.CalendarExcludedName); // Формат расписания по календарю.

                if (taskTrigger.DailyTimeInterval is not null)
                    triggerConfigurator.WithDailyTimeIntervalSchedule(taskTrigger
                        .DailyTimeInterval); //Настройка каждого дня, интервала.
            });
    }
}