namespace Arbitrage.Application.Interfaces;

public interface IArbitrageRepository
{
    Task AddAsync(ArbitrageResult result);
    Task<ArbitrageResult> GetLastResultAsync(string symbol, DateTime before);
    Task<IEnumerable<ArbitrageResult>> GetResultsAsync(string symbol, DateTime from, DateTime to);
}
