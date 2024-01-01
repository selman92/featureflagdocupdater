using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Client;
using ConfigurationBuilder = LaunchDarkly.Sdk.Client.ConfigurationBuilder;

namespace FeatureFlagService.Clients;

public class LaunchDarklyClient : ILaunchDarklyClient
{
    private readonly IConfiguration _configuration;
    private LdClient _devClient;
    private LdClient _testClient;
    private LdClient _prodClient;

    public LaunchDarklyClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IReadOnlyCollection<FeatureFlag> GetAllFlags()
    {
        var devFlags = GetFlags("dev");
        var testFlags = GetFlags("test");
        var prodFlags = GetFlags("prod");

        var allFlags = MapFlags(devFlags, testFlags, prodFlags);

        return allFlags;
    }

    private IReadOnlyCollection<FeatureFlag> MapFlags(IDictionary<string, LdValue> devFlags,
        IDictionary<string, LdValue> testFlags, IDictionary<string, LdValue> prodFlags)
    {
        var flags = new List<FeatureFlag>();

        foreach (var flag in devFlags)
        {
            flags.Add(new FeatureFlag(flag.Key, 
                flag.Value.AsBool, 
                testFlags[flag.Key].AsBool,
                prodFlags[flag.Key].AsBool));
        }

        return flags;
    }

    private IDictionary<string, LdValue> GetFlags(string environment)
    {
        using var client = GetClient(environment);

        return client.AllFlags();
    }

    private LdClient GetClient(string environment)
    {
        var sdkKey = GetSdkKey(environment);
        
        var context = Context.New("default");
        var client = LdClient.Init(sdkKey, ConfigurationBuilder.AutoEnvAttributes.Disabled,
            context, TimeSpan.FromSeconds(15));

        return client;
    }

    private string GetSdkKey(string environment)
    {
        var sdkKey = environment switch
        {
            "dev" => _configuration["MobileSdkKeys:Dev"],
            "test" => _configuration["MobileSdkKeys:Test"],
            "prod" => _configuration["MobileSdkKeys:Prod"],
            _ => throw new ArgumentException("Invalid environment", nameof(environment))
        };

        return sdkKey;
    }
}