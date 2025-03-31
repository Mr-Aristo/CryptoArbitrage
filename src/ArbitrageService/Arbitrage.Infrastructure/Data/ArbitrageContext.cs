namespace Arbitrage.Infrastructure.Data;

public class ArbitrageContext : DbContext
{
    public ArbitrageContext(DbContextOptions<ArbitrageContext> options) : base(options)
    {
    }
    public DbSet<ArbitrageResult> ArbitrageResults { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ArbitrageResult>().HasData(
            new ArbitrageResult
            {
                Id = Guid.NewGuid(),
                TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Symbol = "BTCUSDT_QUARTER",
                PriceDifference = 0.5m
            },
            new ArbitrageResult
            {
                Id = Guid.NewGuid(),
                TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Symbol = "BTCUSDT_BI-QUARTER",
                PriceDifference = 1.0m
            }
        );
    }
}
