namespace Arbitrage.Application.Services;

public class ArbitrageCalculationService : IArbitageCalculation
{
    private readonly IArbitrageRepository _repository;
    private readonly ILogger<ArbitrageCalculationService> _logger;
    public ArbitrageCalculationService(IArbitrageRepository repository, ILogger<ArbitrageCalculationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }


    public async Task<ArbitrageResult> CalculateAndSaveAsync(FuturePriceDto currentPrice, FuturePriceDto previousPrice)
    {
        if (currentPrice.Symbol != previousPrice.Symbol)
        {
            _logger.LogWarning("Symbol mismatch detected. Current: {CurrentSymbol}, Previous: {PreviousSymbol}",
                currentPrice.Symbol, previousPrice.Symbol);
            throw new ArgumentException("Symbols don't match");
        }

        try
        {

            _logger.LogDebug("Calculating arbitrage between {CurrentPrice} and {PreviousPrice}",
                currentPrice.Price, previousPrice.Price);

            var diff = Math.Abs(currentPrice.Price - previousPrice.Price);

            var result = new ArbitrageResult
            {
                TimeStamp = DateTime.UtcNow,
                PriceDifference = diff,
                Symbol = currentPrice.Symbol
            };

            _logger.LogInformation("Arbitrage calculated. Difference: {Difference} for symbol {Symbol}",
                diff, currentPrice.Symbol);

            await _repository.AddAsync(result);

            _logger.LogDebug("Arbitrage result saved successfully. ID: {ResultId}", result.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during arbitrage calculation");
            throw;
        }
    }
}
