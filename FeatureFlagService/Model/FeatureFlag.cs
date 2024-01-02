namespace FeatureFlagService.Model;

public record FeatureFlag(string Name, bool DevStatus, bool TestStatus, bool ProdStatus);