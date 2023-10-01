using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

namespace ReadProjectDetails.Interfaces
{
    public interface IDynamoDbService
    {
        DynamoDbItem DeserializeAndStoreMessage(SQSEvent.SQSMessage message, ILambdaContext context);
    }
}