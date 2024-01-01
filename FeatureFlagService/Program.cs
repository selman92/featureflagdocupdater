using FeatureFlagService;
using FeatureFlagService.Clients;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<FeatureFlagBackgroundService>();

builder.Services.AddSingleton<ILaunchDarklyClient, LaunchDarklyClient>();

var host = builder.Build();
host.Run();