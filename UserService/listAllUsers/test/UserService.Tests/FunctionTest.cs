using Xunit;
using NSubstitute;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;

namespace UserService.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestFunctionHandler()
    {
        // Arrange
        var mockClient = Substitute.For<IAmazonCognitoIdentityProvider>();
        var users = new List<UserType> { new UserType { Username = "testuser" } };
        mockClient
            .ListUsersAsync(Arg.Any<ListUsersRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ListUsersResponse { Users = users });
        var function = new Function(mockClient);
        var context = Substitute.For<ILambdaContext>();

        // Act
        var result = await function.FunctionHandler(context);

        // Assert
        await mockClient.Received().ListUsersAsync(Arg.Is<ListUsersRequest>(request => request.UserPoolId == "us-east-1_YwI4ucIqm"), Arg.Any<CancellationToken>());
        Assert.Equal(users, result);
    }
}
