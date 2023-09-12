using Otel.Data;

namespace Otel.Services;

public class MyHostedService : IHostedService
{
    private int executionCount = 0;
    private readonly ILogger<MyHostedService> _logger;
    private readonly IConfiguration _configuration;
    private Timer? _timer = null;

    public MyHostedService(ILogger<MyHostedService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        using (var dc = new DataContext(_configuration))
        {
            var user = dc.Users.FirstOrDefault(u => u.Age == 15);

            if (user is not null)
            {
                user.Age = 18;
                dc.SaveChanges();
            }
            else
            {
                _logger.LogWarning("no user found");
            }
        }

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}