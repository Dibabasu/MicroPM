using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserService;
public class Function
{
    private readonly IAmazonCognitoIdentityProvider _client;
    private readonly string _userPoolId;

    public Function() : this(new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1)) {


     }

    public Function(IAmazonCognitoIdentityProvider client)
    {
        _client = client;
        _userPoolId = System.Environment.GetEnvironmentVariable("User_Pool_Id")!;
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<List<UserType>> FunctionHandler(ILambdaContext context)
    {
        var request = new ListUsersRequest
        {
            UserPoolId = _userPoolId
        };

        var response = await _client.ListUsersAsync(request);

        return response.Users;
    }

}
