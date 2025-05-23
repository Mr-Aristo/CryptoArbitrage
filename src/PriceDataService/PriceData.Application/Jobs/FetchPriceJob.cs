﻿namespace PriceData.Application.Jobs;

public class FetchPriceJob : IJob
{
    private readonly IFuturePriceService _priceService;
    private readonly ILogger<FetchPriceJob> _logger;
    public FetchPriceJob(IFuturePriceService priceService, ILogger<FetchPriceJob> logger)
    {
        _priceService = priceService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Data flow from Binance API: {Time}", DateTime.UtcNow); 
        await _priceService.FetchPriceAsync("BTCUSDT_QUARTER");
        await _priceService.FetchPriceAsync("BTCUSDT_BI-QUARTER");
    }
}
