using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using PriceData.Application.Interfaces;
using PriceData.Domain.Exceptions;
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

        return priceData;

    }

}