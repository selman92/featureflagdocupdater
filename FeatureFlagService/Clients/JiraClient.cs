using System.Net;
using System.Text.Json;
using FeatureFlagService.Model;
using RestSharp;
using RestSharp.Authenticators;
using Version = FeatureFlagService.Model.Version;

namespace FeatureFlagService.Clients;

public class JiraClient : IJiraClient
{
    private readonly string? _apiToken;
    private readonly Uri _jiraBaseUri;
    private readonly string? _username;
    
    private const int RequestTimeout = 120000;
    
    private const string UpdateDocumentEndpoint = "/wiki/rest/api/content/{0}";
    public JiraClient(IConfiguration configuration)
    {
        _username = configuration["Jira:Username"];
        _apiToken = configuration["Jira:ApiToken"];
        _jiraBaseUri = new Uri(configuration["Jira:Url"]);
    }
    
    public async Task<bool> UpdateDocument(string documentId, string title, string content)
    {
        var version = await GetDocumentVersion(documentId) + 1;
        var restClient = GetRestClient();
        
        var request = new RestRequest(new Uri(string.Concat(_jiraBaseUri.AbsoluteUri.Trim('/'), string.Format(UpdateDocumentEndpoint, documentId))),
            Method.Put);

        var updateDocumentModel = new UpdateDocumentModel
        {
            id = documentId,
            type = "page",
            title = title,
            space = new Space { key = "AE" },
            body = new Body { storage = new Storage { value = content, representation = "storage" } },
            version = new Version { number = version }
        };

        request.Authenticator = new HttpBasicAuthenticator(_username, _apiToken);
        request.Timeout = RequestTimeout;

        request.AddJsonBody(updateDocumentModel, "application/json");

        try
        {
            var response = await restClient.ExecuteAsync(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return false;
    }

    private async Task<int> GetDocumentVersion(string documentId)
    {
        var restClient = GetRestClient();
        
        var request = new RestRequest(new Uri(string.Concat(_jiraBaseUri.AbsoluteUri.Trim('/'), string.Format(UpdateDocumentEndpoint, documentId))));
        request.Authenticator = new HttpBasicAuthenticator(_username, _apiToken);
        try
        {
            var response = await restClient.ExecuteAsync(request);

            var doc = JsonSerializer.Deserialize<Document>(response.Content);

            return doc.version.number;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 1;
        }
    }

    private RestClient GetRestClient()
    {
        var restClient = new RestClient(_jiraBaseUri);

        return restClient;
    }
}