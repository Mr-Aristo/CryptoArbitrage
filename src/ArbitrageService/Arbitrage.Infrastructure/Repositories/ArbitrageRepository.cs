namespace Arbitrage.Infrastructure.Repositories;

public class ArbitrageRepository : IArbitrageRepository
{
    private readonly ArbitrageContext _context;
    public ArbitrageRepository(ArbitrageContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ArbitrageResult result)
    {
        await _context.ArbitrageResults.AddAsync(result);
        await _context.SaveChangesAsync();
    }

    public async Task<ArbitrageResult> GetLastResultAsync(string symbol, DateTime before)
    {
        return await _context.ArbitrageResults
              .Where(r => r.TimeStamp < before && r.Symbol == symbol)
              .OrderByDescending(r => r.TimeStamp)
              .AsNoTracking()
              .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ArbitrageResult>> GetResultsAsync(string symbol, DateTime from, DateTime to)
    {
        return await _context.ArbitrageResults
           .Where(r => r.TimeStamp >= from && r.TimeStamp <= to && r.Symbol == symbol)
           .AsNoTracking()
           .ToListAsync();
    }
}
