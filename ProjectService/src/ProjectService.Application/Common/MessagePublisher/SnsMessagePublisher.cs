using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using ProjectService.Application.Common.MessagePublisher.Interfaces;
using ProjectService.Application.Common.Settings;
using ProjectService.Domain.Event;
using Serilog;

namespace ProjectService.Application.Common.MessagePublisher;

public class SnsMessagePublisher : IMessagePublisher
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly ILogger _logger;
    private readonly IOptions<TopicSettings> _topicSettings;
    private string? _topicArn;

    public SnsMessagePublisher(IOptions<TopicSettings> topicSettings, IAmazonSimpleNotificationService sns)
    {
        _topicSettings = topicSettings;
        _sns = sns;
        _logger = Log.ForContext<SnsMessagePublisher>();
    }
    private async Task<string> GetTopicArnAsync()
    {
        try
        {
            var queueUrlResponse = await _sns.FindTopicAsync(_topicSettings.Value.Name);
            _topicArn = queueUrlResponse.TopicArn;
            return _topicArn;
        }
        catch (System.Exception ex)
        {
            _logger.Error(ex, "Failed to find topic url");
            throw new Exception("Failed to find topic url", ex);
        }
    }
    public async Task Publish<T>(T notification)
    {
        try
        {
            var topicArn = await GetTopicArnAsync();

            var sendMessageRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = JsonSerializer.Serialize(notification),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "MessageType", new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name
                        }
                    }
                }
            };
            var response = await _sns.PublishAsync(sendMessageRequest);
            _logger.Information($"Messge Queued {response}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "PublishMessageAsync failed");
        }
    }
}