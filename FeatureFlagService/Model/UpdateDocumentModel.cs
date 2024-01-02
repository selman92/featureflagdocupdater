namespace FeatureFlagService.Model;

public class UpdateDocumentModel
{
    public string id { get; set; }
    public string type { get; set; }
    public string title { get; set; }
    public Space space { get; set; }
    public Body body { get; set; }
    public Version version { get; set; }
}