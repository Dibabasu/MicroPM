using ProjectService.Application.Common.MessagePublisher.Interfaces;

namespace ProjectService.Application.Common.MessagePublisher;

public class RabbitMQPublisher : IMessagePublisher
{
    public Task Publish<T>(T notification)
    {
        throw new NotImplementedException();
    }
}
