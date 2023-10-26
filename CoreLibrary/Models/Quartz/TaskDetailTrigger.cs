using Quartz;

namespace CoreLibrary.Models.Quartz;

public record TaskDetailTrigger
{
    public string NameTrigger { get; set; }
    public string Description { get; set; }
    public string NameJob { get; set; }
    public DateTimeOffset StartTime { get; set; } = DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddMinutes(5));
    public string CalendarExcludedName { get; set; }
    public Action<CalendarIntervalScheduleBuilder>? CallendarInterval { get; set; }
    public Action<SimpleScheduleBuilder>? SimpleInterval { get; set; }
    public Action<DailyTimeIntervalScheduleBuilder>? DailyTimeInterval { get; set; }
}