using ProjectService.Application.Common.MessagePublisher.Interfaces;

namespace ProjectService.Application.Common.MessagePublisher;

public class AzureMessagePublisher : IMessagePublisher
{
    public Task Publish<T>(T notification,string topicName)
    {
        throw new NotImplementedException();
    }
}