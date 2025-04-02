namespace RabbitMQ.Services;


public class RabbitMqService : IRabbitMqService, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMqService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:Host"],
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"], 
        };

 
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishMessageAsync<T>(T message, string queueName)
    {
        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        await _channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
    }

    public async Task SubscribeToQueueAsync<T>(string queueName, Func<T, Task> onMessageReceived)
    {
        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var obj = JsonSerializer.Deserialize<T>(message);
            if (obj != null)
            {
                await onMessageReceived(obj);
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
        }
        if (_connection != null)
        {
            await _connection.CloseAsync();
        }
    }
}
