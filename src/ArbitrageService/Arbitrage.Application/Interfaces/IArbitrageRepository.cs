using Arbitrage.Domain.Models;


namespace Arbitrage.Application.Interfaces;

public interface IArbitrageRepository
{
    Task AddAsync(ArbitrageResult result);
    Task<ArbitrageResult> GetLastResultAsync(DateTime before);
    Task<IEnumerable<ArbitrageResult>> GetResultsAsync(DateTime from, DateTime to);
}
