using Quartz;
using Serilog;

namespace CoreLibrary.Models.Quartz.Listeners;

/// <summary>
///     Слушатель событий триггеров по умолчанию.
/// </summary>
public class DefaultTriggerListener : ITriggerListener
{
    private readonly ILogger _logger;

    /// <summary>
    ///     Инициализирует новый экземпляр класса DefaultTriggerListener.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public DefaultTriggerListener(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Вызывается, когда триггер срабатывает для задачи.
    /// </summary>
    public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
        CancellationToken cancellationToken = new())
    {
        _logger.Information($"Триггер '{trigger.Key}' сработал для задачи '{context.JobDetail.Key}'.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается для проверки возможности выполнения задачи триггером.
    /// </summary>
    public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
        CancellationToken cancellationToken = new())
    {
        _logger.Information($"Исполнение задачи '{context.JobDetail.Key}' отклонено триггером '{trigger.Key}'.");
        return false; // По умолчанию не отклонять исполнение задачи
    }

    /// <summary>
    ///     Вызывается, когда триггер пропущен.
    /// </summary>
    public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new())
    {
        _logger.Warning($"Триггер '{trigger.Key}' пропущен.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда триггер завершает выполнение задачи.
    /// </summary>
    public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context,
        SchedulerInstruction triggerInstructionCode,
        CancellationToken cancellationToken = new())
    {
        _logger.Information($"Триггер '{trigger.Key}' завершил выполнение задачи '{context.JobDetail.Key}'. " +
                            $"Код инструкции: {triggerInstructionCode}");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Имя слушателя триггеров.
    /// </summary>
    public string Name => "DefaultTrigger_Listener";
}