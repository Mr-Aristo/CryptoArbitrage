using Arbitrage.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arbitrage.Infrastructure.Data;

public class ArbitrageContext : DbContext
{
	public ArbitrageContext(DbContextOptions<ArbitrageContext> options) : base(options)	
	{
	}

	public DbSet<ArbitrageResult> ArbitrageResults { get; set; }
}
