using FeatureFlagService.Model;

namespace FeatureFlagService.Clients;

public interface ILaunchDarklyClient
{
    IReadOnlyCollection<FeatureFlag> GetAllFlags();
}