  namespace RabbitMQ.Interfaces;

public interface IMessageService
{
    Task PublishMessageAsync<T>(T message);
}
