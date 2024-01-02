namespace FeatureFlagService.Clients;

public interface IJiraClient
{
    Task<bool> UpdateDocument(string documentId, string title, string content);
}