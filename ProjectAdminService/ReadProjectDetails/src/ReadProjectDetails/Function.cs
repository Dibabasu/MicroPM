using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ReadProjectDetails;

public class Function
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly DynamoDBContext _context;
    private readonly string _tableName;
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        _dynamoDb = new AmazonDynamoDBClient();
        _context = new DynamoDBContext(_dynamoDb);
        _tableName = System.Environment.GetEnvironmentVariable("DYNAMODB_TABLE_NAME")!;
    }
    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaFunction]
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            try
            {
                await ProcessMessageAsync(message, context);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing message {message.MessageId}: {ex.Message}");
            }
        }
    }
    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processing message {message.Body}");

        try
        {
            // Deserialize the SQS message
            var sqsMessage = JsonSerializer.Deserialize<SqsMessage>(message.Body);

            // Deserialize the project message
            var projectMessage = JsonSerializer.Deserialize<ProjectMessage>(sqsMessage!.Message);

            ///write code to store details into dynamo db table
            var dynamoDbItem = new DynamoDbItem
            {
                Id = message.MessageId,
                ProjectName = projectMessage!.Project.ProjectDetails.Name,
                ProjectDescription = projectMessage.Project.ProjectDetails.Description,
                OwnerId = projectMessage.Project.OwnerId,
                ProjectId = projectMessage.Project.Id,
                ProjectStatus = projectMessage.Project.ProjectStatus,
                Message = message.Body,
                IsProccessed = false

            };
            await _context.SaveAsync(dynamoDbItem, new DynamoDBOperationConfig
            {
                OverrideTableName = _tableName
            });
        }
        catch (JsonException ex)
        {
            context.Logger.LogError($"Error deserializing message {message.Body}: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error processing message {message.Body}: {ex.Message}");
            throw;
        }
    }
}
public class DynamoDbItem
{
    [DynamoDBHashKey]
    public string Id { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string Message { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string ProjectName { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string ProjectDescription { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string OwnerId { get; set; } = string.Empty;
    [DynamoDBProperty]
    public string ProjectId { get; set; } = string.Empty;
    [DynamoDBProperty]
    public int ProjectStatus { get; set; }
    [DynamoDBProperty]
    public bool IsProccessed { get; set; }

}
