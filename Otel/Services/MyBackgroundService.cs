namespace Otel.Services;

public class MyBackgroundService : BackgroundService
{
    private readonly ILogger<MyBackgroundService> _logger;

    public MyBackgroundService(ILogger<MyBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MyBackgroundService running.");

        await Task.Delay(1000, stoppingToken);

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MyBackgroundService is working.");

        int counter = 0;

        while (true)
        {
            _logger.LogInformation($"Work was done already {counter++} times");
            await Task.Delay(5000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MyBackgroundService is stopping.");

        await base.StopAsync(stoppingToken);
    }
}