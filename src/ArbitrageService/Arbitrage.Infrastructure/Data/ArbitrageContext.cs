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
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Symbol = "BTCUSDT_QUARTER",
                PriceDifference = 0.5m
            },
            new ArbitrageResult
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Symbol = "BTCUSDT_BI-QUARTER",
                PriceDifference = 1.0m
            }
        );
    }
}
