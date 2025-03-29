using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using PriceData.Application.Interfaces;
using PriceData.Domain.Models;

public class FuturesPriceService
{
    private readonly IBinanceRestClient _restClient;
    private readonly IFuturePriceRepository _repository;

    public FuturesPriceService(IConfiguration config, IFuturePriceRepository repository)
    {
        var apiKey = config["BinanceApi:ApiKey"];
        var apiSecret = config["BinanceApi:ApiSecret"];

        // REST client setup
        _restClient = new BinanceRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
        });

        _repository = repository;
    }

    public async Task<FuturePrice> FetchPriceAsync(string symbol)
    {
        // Futures price check
        var result = await _restClient.UsdFuturesApi.ExchangeData.GetPriceAsync(symbol);

        if (!result.Success)
            throw new Exception($"Price error: {result.Error}");

        var priceData = new FuturePrice
        {
            Symbol = symbol,
            Price = result.Data.Price,
            TimeStamp = DateTime.UtcNow
        };

        await _repository.AddAsync(priceData);
        return priceData;
    }

}