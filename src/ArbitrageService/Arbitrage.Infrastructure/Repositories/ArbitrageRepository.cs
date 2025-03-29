
using Arbitrage.Application.Interfaces;
using Arbitrage.Domain.Models;
using Arbitrage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ArbitrageResult> GetLastResultAsync(DateTime before)
    {
        return await _context.ArbitrageResults
              .Where(r => r.TimeStamp < before)
              .OrderByDescending(r => r.TimeStamp)
              .AsNoTracking()
              .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ArbitrageResult>> GetResultsAsync(DateTime from, DateTime to)
    {
        return await _context.ArbitrageResults
           .Where(r => r.TimeStamp >= from && r.TimeStamp <= to)
           .AsNoTracking()
           .ToListAsync();
    }
}
