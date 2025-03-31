namespace Arbitrage.Application.Services;

public class PriceDataConsumer : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IArbitageCalculation _calculationService;
    private readonly ILogger<PriceDataConsumer> _logger;
    private readonly string _queueName;
    private readonly ConcurrentDictionary<string, FuturePriceDto> _priceBuffer;

    public PriceDataConsumer(IRabbitMqService rabbitMqService,
        IArbitageCalculation calculationService,
        IConfiguration configuration,
        ILogger<PriceDataConsumer> logger)
    {
        _rabbitMqService = rabbitMqService;
        _calculationService = calculationService;
        _logger = logger;
        _queueName = configuration["RabbitMQ:PriceDataQueue"] ?? "PriceDataQueue";
        _priceBuffer = new ConcurrentDictionary<string, FuturePriceDto>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        await _rabbitMqService.SubscribeToQueueAsync<FuturePriceDto>(_queueName, async (priceData) =>
        {
            _logger.LogInformation("Received price data: {Symbol} - {Price}", priceData.Symbol, priceData.Price);
            _priceBuffer[priceData.Symbol] = priceData;


            if (_priceBuffer.TryGetValue("BTCUSDT_QUARTER", out var currentPrice) &&
            _priceBuffer.TryGetValue("BTCUSDT_BI-QUARTER", out var previousPrice))
            {
                try
                {
                    _logger.LogInformation("Both price data received. Calculating arbitrage...");

                   
                    if (currentPrice != null && previousPrice != null)
                    {
                        await _calculationService.CalculateAndSaveAsync(currentPrice, previousPrice);
                    }
                    else
                    {
                        _logger.LogWarning("One or both price values are null. Skipping calculation.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during arbitrage calculation.");
                }
                finally
                {

                    if (_priceBuffer.Count > 0)
                    {
                        _priceBuffer.Clear();
                    }
                }
            }
            else
            {
                
                var missingKeys = new List<string>();
                if (!_priceBuffer.ContainsKey("BTCUSDT_QUARTER")) missingKeys.Add("BTCUSDT_QUARTER");
                if (!_priceBuffer.ContainsKey("BTCUSDT_BI-QUARTER")) missingKeys.Add("BTCUSDT_BI-QUARTER");

                _logger.LogDebug($"Waiting for price data. Missing keys: {string.Join(", ", missingKeys)}");
            }
        });
    }
}
