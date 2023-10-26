using Quartz;

namespace CoreLibrary.Models.Quartz;

public record TaskDetailJob
{
    public required JobKey JobIdentity { get; set; }
    public required string Description { get; set; }
    public bool JobAfterInDataHistory { get; set; } = true;
    public bool JobDeleteIfNotTrigger { get; set; } = false;
    public bool NotConcurrentExecute { get; set; } = true;
    public required Type JobType { get; set; }
}