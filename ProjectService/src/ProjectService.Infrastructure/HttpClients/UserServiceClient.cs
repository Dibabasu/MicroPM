using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Settings;
using ProjectService.Infrastructure.HttpClients.Models;

namespace ProjectService.Infrastructure.HttpClients;

public class UserServiceClient
{
    private readonly HttpClient _httpClient;
    public string Stage { get; }

    public UserServiceClient(HttpClient httpClient, IOptions<UserServiceSettings> settings)
    {
        _httpClient = httpClient;
        Stage = settings.Value.Stage;
    }

    public async Task<UserModel> GetUser(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/{Stage}/userbyuserid?userId={userId}", cancellationToken);
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserModel>(cancellationToken: cancellationToken);
            return user ?? throw new NotFoundException($"User with id {userId} not found.");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new UserModel(
                    Email: "default@user.com", 
                    Sid: "00000000-0000-0000-0000-000000000000");
            }
            throw new Exception("Error while getting user", ex);
        }
    }
}
