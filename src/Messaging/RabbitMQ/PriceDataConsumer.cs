using System.Text;
using System.Text.Json;
using Arbitrage.Application.DTOs;
using Arbitrage.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArbitrageService.Messaging;

public class PriceDataConsumer : BackgroundService
{
    private readonly ILogger<PriceDataConsumer> _logger;
    private readonly IConfiguration _configuration;
    private readonly ArbitrageCalculationService _calculationService;
    private IConnection? _connection;
    private IChannel? _channel; 
    private readonly string _queueName;

    public PriceDataConsumer(ILogger<PriceDataConsumer> logger, IConfiguration configuration, ArbitrageCalculationService calculationService)
    {
        _logger = logger;
        _configuration = configuration;
        _calculationService = calculationService;
        _queueName = _configuration["RabbitMQ:QueueName"];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ consumer initiating...");

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:Host"],
            UserName = _configuration["RabbitMQ:UserName"],
            Password = _configuration["RabbitMQ:Password"]
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync(); 

        await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var priceData = JsonSerializer.Deserialize<FuturePriceDto>(message);
                if (priceData != null)
                {
                    _logger.LogInformation("Recived message: {Symbol} - {Price} at {Time}", priceData.Symbol, priceData.Price, priceData.TimeStamp);
                    await ProcessPriceData(priceData);
                }
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the message");
            }
        };

        await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer); 

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ProcessPriceData(FuturePriceDto priceData)
    {
        var timeKey = new DateTime(priceData.TimeStamp.Year, priceData.TimeStamp.Month, priceData.TimeStamp.Day,
                                   priceData.TimeStamp.Hour, priceData.TimeStamp.Minute, 0);

        _logger.LogInformation("Price data has been processed: {Symbol} - {Price} at {Time}", priceData.Symbol, priceData.Price, timeKey);
        await _calculationService.CalculateAndSaveAsync(priceData, priceData);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync(); 
        }

        await base.StopAsync(stoppingToken);
    }
}
