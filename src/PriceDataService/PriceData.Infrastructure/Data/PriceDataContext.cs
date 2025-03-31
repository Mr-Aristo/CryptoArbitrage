using Microsoft.EntityFrameworkCore;
using PriceData.Domain.Models;

namespace PriceData.Infrastructure.Data;

public class PriceDataContext : DbContext 
{
	public PriceDataContext(DbContextOptions<PriceDataContext> options) : base(options)
	{
	}

    public DbSet<FuturePrice> FuturePrices { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FuturePrice>().HasData(
            new FuturePrice
            {
                Id = Guid.NewGuid(),
                Symbol = "BTCUSDT_QUARTER",
                Price = 45000.50m,
                TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new FuturePrice
            {
                Id = Guid.NewGuid(),
                Symbol = "BTCUSDT_BI-QUARTER",
                Price = 3000.25m,
                TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
