namespace Arbitrage.Domain.Models;

public class ArbitrageResult
{
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Symbol { get; set; }
    public decimal PriceDifference { get; set; }
}
