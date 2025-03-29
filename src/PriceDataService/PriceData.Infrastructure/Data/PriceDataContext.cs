using Microsoft.EntityFrameworkCore;
using PriceData.Domain.Models;

namespace PriceData.Infrastructure.Data;

public class PriceDataContext : DbContext 
{
	public PriceDataContext(DbContextOptions<PriceDataContext> options) : base(options)
	{
	}

    public DbSet<FuturePrice> FuturePrices { get; set; }
}
