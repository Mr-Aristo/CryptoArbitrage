
using Arbitrage.Application.DTOs;
using Arbitrage.Application.Interfaces;
using Arbitrage.Domain.Models;

namespace Arbitrage.Application.Services;

public class ArbitrageCalculationService
{
    private readonly IArbitrageRepository _repository;
    public ArbitrageCalculationService(IArbitrageRepository repository)
    {
        _repository = repository;
    }

 
    public async Task<ArbitrageResult> CalculateAndSaveAsync(FuturePriceDto firstPrice, FuturePriceDto secondPrice)
    {
        
        var diff = Math.Abs(firstPrice.Price - secondPrice.Price);
        var result = new ArbitrageResult
        {
            TimeStamp = DateTime.UtcNow,
            PriceDifference = diff
        };
        await _repository.AddAsync(result);
        return result;
    }
}
