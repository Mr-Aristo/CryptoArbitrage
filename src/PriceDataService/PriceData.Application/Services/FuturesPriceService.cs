namespace PriceData.Application.Services;

public class FuturesPriceService : IFuturePriceService
{
    private readonly IBinanceRestClient _restClient;
    private readonly IFuturePriceRepository _repository;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly string _priceDataQueue;

    public FuturesPriceService(IConfiguration configuration, IFuturePriceRepository repository, IRabbitMqService rabbitMqService)
    {
        var apiKey = configuration["BinanceApi:ApiKey"];
        var apiSecret = configuration["BinanceApi:ApiSecret"];

        // REST client setup
        _restClient = new BinanceRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
        });

        _repository = repository;
        _rabbitMqService = rabbitMqService;
        _priceDataQueue = configuration["RabbitMQ:PriceDataQueue"] ?? "PriceDataQueue";
    }

    public async Task<FuturePrice> FetchPriceAsync(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new InvalidSymbolException(symbol);
        }

       
        var result = await _restClient.UsdFuturesApi.ExchangeData.GetPriceAsync(symbol);

        if (!result.Success)
        {
          
            throw new ExternalApiException($"Price error: {result.Error}");
        }

        var priceData = new FuturePrice
        {
            Symbol = symbol,
            Price = result.Data.Price,
            TimeStamp = DateTime.UtcNow
        };

        try
        {
            await _repository.AddAsync(priceData);
        }
        catch (Exception ex)
        {
            throw new DatabaseException($"Occured an error while saving price: {ex.Message}", ex);
        }

        await _rabbitMqService.PublishMessageAsync(priceData, _priceDataQueue);

        return priceData;

    }

}