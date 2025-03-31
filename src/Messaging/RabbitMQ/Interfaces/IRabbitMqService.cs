namespace RabbitMQ.Interfaces;

public interface IRabbitMqService
{
    Task PublishMessageAsync<T>(T message, string queueName);
    Task SubscribeToQueueAsync<T>(string queueName, Func<T, Task> onMessageReceived);
}

