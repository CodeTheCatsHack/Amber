namespace CoreLibrary.Models.Quartz;

public record TaskExcludedCallendar
{
    public required string ShedulerName { get; set; }
    public required string CallendarName { get; set; }
    public bool Replace { get; set; } = false;
    public bool UpdateTriggers { get; set; } = false;
    public DateTime ExcludedDate { get; set; }
}