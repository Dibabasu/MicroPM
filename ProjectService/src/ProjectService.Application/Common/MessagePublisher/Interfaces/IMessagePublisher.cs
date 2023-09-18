using ProjectService.Domain.Event;

namespace ProjectService.Application.Common.MessagePublisher.Interfaces;
public interface IMessagePublisher
{
    Task Publish<T>(T notification,string topicName);
}