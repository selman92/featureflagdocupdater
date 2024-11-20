# Feature Flag Confluence Synchronizer

## Overview
.NET application that synchronizes LaunchDarkly feature flags to a Confluence document, tracking state changes and updating periodically.

## Configuration

### AppSettings.json Structure
```json
{
  "MobileSdkKeys": {
    "Dev": "",
    "Test": "",
    "Prod": ""
  },
  "Jira": {
    "Username": "",
    "ApiToken": "",
    "Url": ""
  }
}
```

## Prerequisites
- .NET Core SDK
- LaunchDarkly Account
- Confluence Access

## Features
- Fetch feature flags from LaunchDarkly
- Update Confluence document
- Track flag state changes
- Support for multiple environments (Dev/Test/Prod)

## Setup
1. Clone repository
2. Configure `appsettings.json` with LaunchDarkly and Confluence credentials
3. Install dependencies using `dotnet restore`
4. Build project using `dotnet build`
5. Run application with `dotnet run`

## Dependencies
- LaunchDarkly SDK
- Confluence API Client

## How It Works
- Periodically retrieves feature flags from LaunchDarkly
- Compares current flag states with previous states
- Updates Confluence document with flag changes
- Logs state transitions and updates
