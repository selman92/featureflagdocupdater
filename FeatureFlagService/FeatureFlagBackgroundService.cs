using FeatureFlagService.Clients;

namespace FeatureFlagService;

public class FeatureFlagBackgroundService : BackgroundService
{
    private readonly ILaunchDarklyClient _launchDarklyClient;
    private readonly ILogger<FeatureFlagBackgroundService> _logger;

    public FeatureFlagBackgroundService(ILaunchDarklyClient launchDarklyClient, ILogger<FeatureFlagBackgroundService> logger)
    {
        _launchDarklyClient = launchDarklyClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var allFlags = _launchDarklyClient.GetAllFlags();
            
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}

public record FeatureFlag(string Name, bool DevStatus, bool TestStatus, bool ProdStatus);