using FeatureFlagService.Clients;
using FeatureFlagService.Model;

namespace FeatureFlagService;

public class FeatureFlagBackgroundService : BackgroundService
{
    private readonly ILaunchDarklyClient _launchDarklyClient;
    private readonly IJiraClient _jiraClient;
    private readonly IDocumentGenerator _documentGenerator;
    private readonly ILogger<FeatureFlagBackgroundService> _logger;

    private const string DocumentId = "348587459";
    private const string DocumentTitle = "Feature Flags";

    private IReadOnlyCollection<FeatureFlag> _latestFlags;
    
    public FeatureFlagBackgroundService(ILaunchDarklyClient launchDarklyClient, IJiraClient jiraClient, IDocumentGenerator documentGenerator, ILogger<FeatureFlagBackgroundService> logger)
    {
        _launchDarklyClient = launchDarklyClient;
        _jiraClient = jiraClient;
        _documentGenerator = documentGenerator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var allFlags = _launchDarklyClient.GetAllFlags();
                _logger.LogInformation($"Fetched {allFlags.Count} flags from LaunchDarkly");

                if (_latestFlags != null && !IsUpdateRequired(allFlags))
                {
                    _latestFlags = allFlags;
                    continue;
                }
                
                var docContent = _documentGenerator.Generate(allFlags);

                await _jiraClient.UpdateDocument(DocumentId, DocumentTitle, docContent);
                _logger.LogInformation("Document successfully updated in confluence.");
                
                _latestFlags = allFlags;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while fetching flags from LaunchDarkly");
            }
        }
    }

    private bool IsUpdateRequired(IReadOnlyCollection<FeatureFlag> currentFlags)
    {
        return !_latestFlags.OrderBy(f => f.Name).SequenceEqual(currentFlags.OrderBy(f => f.Name));
    }
}