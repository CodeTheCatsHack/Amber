using Quartz;
using Serilog;

namespace CoreLibrary.Models.Quartz.Listeners;

/// <summary>
///     Слушатель событий задач по умолчанию.
/// </summary>
public class DefaultJobListener : IJobListener
{
    private readonly ILogger _logger;

    /// <summary>
    ///     Инициализирует новый экземпляр класса DefaultJobListener.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public DefaultJobListener(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Вызывается, когда задача собирается выполниться.
    /// </summary>
    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Задача '{context.JobDetail.Key}' собирается выполниться.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда выполнение задачи отклонено.
    /// </summary>
    public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new())
    {
        _logger.Information($"Исполнение задачи '{context.JobDetail.Key}' отклонено.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Вызывается, когда задача успешно выполнена или возникла ошибка при выполнении.
    /// </summary>
    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
        CancellationToken cancellationToken = new())
    {
        if (jobException == null)
            _logger.Information($"Задача '{context.JobDetail.Key}' успешно выполнена.");
        else
            _logger.Error(jobException, $"Ошибка при выполнении задачи '{context.JobDetail.Key}'.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Имя слушателя задач.
    /// </summary>
    public string Name => "DefaultJobListener";
}