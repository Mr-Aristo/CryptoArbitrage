using MassTransit;
namespace Messaging.Services;

public interface IMessageService
{
        Task PublishMessageAsync<T>(T message);
}

public class MassTransitMessageService : IMessageService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishMessageAsync<T>(T message)
    {
        await _publishEndpoint.Publish(message);
    }
}

