using FeatureFlagService.Model;

namespace FeatureFlagService;

public interface IDocumentGenerator
{
    string Generate(IReadOnlyCollection<FeatureFlag> featureFlags);
}