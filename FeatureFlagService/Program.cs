using FeatureFlagService;
using FeatureFlagService.Clients;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<FeatureFlagBackgroundService>();

builder.Services.AddSingleton<ILaunchDarklyClient, LaunchDarklyClient>();
builder.Services.AddSingleton<IJiraClient, JiraClient>();
builder.Services.AddSingleton<IDocumentGenerator, DocumentGenerator>();


var host = builder.Build();
host.Run();