using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Settings;
using ProjectService.Infrastructure.HttpClients.Models;

namespace ProjectService.Infrastructure.HttpClients;

public class UserGroupServiceClient
{

    private readonly HttpClient _httpClient;
    public string Stage { get; }

    public UserGroupServiceClient(HttpClient httpClient, IOptions<UserServiceSettings> settings)
    {
        _httpClient = httpClient;
        Stage = settings.Value.Stage;
    }
    public async Task<List<UserModel>> GetUsersByGroup(string groupname, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/{Stage}/getuserbygroup/{groupname}", cancellationToken);
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserModel>>(cancellationToken: cancellationToken);
            return users ?? throw new NotFoundException($"Group with name {groupname} not found.");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return (List<UserModel>)Enumerable.Empty<UserModel>();
            }
            throw new Exception("Error while getting user", ex);
        }
    }
}