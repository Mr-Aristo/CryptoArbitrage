using PriceData.Domain.Models;

namespace PriceData.Application.Interfaces;

public interface IFuturePriceService
{
    public Task<FuturePrice> FetchPriceAsync(string symbol);
}
