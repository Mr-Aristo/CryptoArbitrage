using Arbitrage.Application.DTOs;
using Arbitrage.Domain.Models;

namespace Arbitrage.Application.Interfaces;

public interface IArbitageCalculation
{
    public Task<ArbitrageResult> CalculateAndSaveAsync(FuturePriceDto currentPrice, FuturePriceDto previousPrice);
}
