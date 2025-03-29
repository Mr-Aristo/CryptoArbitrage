using Microsoft.EntityFrameworkCore;
using PriceData.Application.Interfaces;
using PriceData.Domain.Models;
using PriceData.Infrastructure.Data;

namespace PriceData.Infrastructure.Repositories;

public class FuturePriceRepository : IFuturePriceRepository
{
    private readonly PriceDataContext _context;
    public FuturePriceRepository(PriceDataContext context)
    {
        _context = context;
    }
    public async Task AddAsync(FuturePrice price)
    {
        await _context.AddAsync(price);
        await _context.SaveChangesAsync();
    }

    public async Task<FuturePrice> GetLastPriceAsync(string symbol, DateTime before)
    {
        return await _context.FuturePrices
              .Where(p => p.Symbol == symbol && p.TimeStamp < before)
              .OrderByDescending(p => p.TimeStamp)
              .AsNoTracking()
              .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<FuturePrice>> GetPricesAsync(string symbol, DateTime from, DateTime to)
    {
        return await _context.FuturePrices
             .Where(p => p.Symbol == symbol && p.TimeStamp >= from && p.TimeStamp <= to)
             .AsNoTracking()
             .ToListAsync();
    }
}
