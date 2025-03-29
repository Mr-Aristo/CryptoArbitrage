using PriceData.Domain.Models;

namespace PriceData.Application.Interfaces;

public interface IFuturePriceRepository
{
    Task AddAsync(FuturePrice price);
    Task<FuturePrice> GetLastPriceAsync(string symbol, DateTime before);
    Task<IEnumerable<FuturePrice>> GetPricesAsync(string symbol, DateTime from, DateTime to);
}