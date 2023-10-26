using Quartz;
using Serilog;

namespace CoreLibrary.Models.Quartz.Listeners;

/// <summary>
///     Слушатель событий планировщика по умолчанию.
/// </summary>
public class DefaultSchedulerListener : ISchedulerListener
{
    private readonly ILogger _logger;

    /// <summary>
    ///     Инициализирует новый экземпляр класса DefaultSchedulerListener.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public DefaultSchedulerListener(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Вызывается, когда задача запланирована по триггеру.
    /// </summary>
    public async Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Запланирована задача по триггеру '{trigger.Key}'.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда запланированная задача отменена.
    /// </summary>
    public async Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Задача, запланированная по триггеру '{triggerKey}', была отменена.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда триггер завершен.
    /// </summary>
    public async Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Триггер '{trigger.Key}' завершен.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда триггер приостановлен.
    /// </summary>
    public async Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Триггер '{triggerKey}' приостановлен.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда все триггеры в группе приостановлены.
    /// </summary>
    public async Task TriggersPaused(string? triggerGroup, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Все триггеры в группе '{triggerGroup}' приостановлены.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда триггер возобновлен.
    /// </summary>
    public async Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Триггер '{triggerKey}' возобновлен.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда все триггеры в группе возобновлены.
    /// </summary>
    public async Task TriggersResumed(string? triggerGroup, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Все триггеры в группе '{triggerGroup}' возобновлены.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда задача добавлена.
    /// </summary>
    public async Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Добавлена задача '{jobDetail.Key}'.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда задача удалена.
    /// </summary>
    public async Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Задача '{jobKey}' удалена.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда задача приостановлена.
    /// </summary>
    public async Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Задача '{jobKey}' приостановлена.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда задача прервана.
    /// </summary>
    public async Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Задача '{jobKey}' прервана.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда все задачи в группе приостановлены.
    /// </summary>
    public async Task JobsPaused(string jobGroup, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Все задачи в группе '{jobGroup}' приостановлены.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда задача возобновлена.
    /// </summary>
    public async Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Задача '{jobKey}' возобновлена.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда все задачи в группе возобновлены.
    /// </summary>
    public async Task JobsResumed(string jobGroup, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Все задачи в группе '{jobGroup}' возобновлены.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается при ошибке планировщика.
    /// </summary>
    public async Task SchedulerError(string msg, SchedulerException cause,
        CancellationToken cancellationToken = new())
    {
        _logger.Error(cause, $"Ошибка планировщика. Сообщение: {msg}");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда планировщик находится в режиме ожидания.
    /// </summary>
    public async Task SchedulerInStandbyMode(CancellationToken cancellationToken = new())
    {
        _logger.Information("Планировщик находится в режиме ожидания.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда планировщик запущен.
    /// </summary>
    public async Task SchedulerStarted(CancellationToken cancellationToken = new())
    {
        _logger.Information("Планировщик запущен.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда начинается запуск планировщика.
    /// </summary>
    public async Task SchedulerStarting(CancellationToken cancellationToken = new())
    {
        _logger.Information("Начало запуска планировщика.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда планировщик выключен.
    /// </summary>
    public async Task SchedulerShutdown(CancellationToken cancellationToken = new())
    {
        _logger.Information("Планировщик выключен.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда начинается выключение планировщика.
    /// </summary>
    public async Task SchedulerShuttingdown(CancellationToken cancellationToken = new())
    {
        _logger.Information("Планировщик выключается.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда данные о планировании очищены.
    /// </summary>
    public async Task SchedulingDataCleared(CancellationToken cancellationToken = new())
    {
        _logger.Information("Данные о планировании очищены.");
        await Task.CompletedTask;
    }
}