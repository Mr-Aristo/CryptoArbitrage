using MassTransit;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Arbitrage.Application.MassTransitConsumers
{
    public class PriceDataConsumer : IConsumer<FuturePriceDto>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PriceDataConsumer> _logger;
        private readonly ConcurrentDictionary<string, FuturePriceDto> _priceBuffer;

        public PriceDataConsumer(IServiceProvider serviceProvider, ILogger<PriceDataConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _priceBuffer = new ConcurrentDictionary<string, FuturePriceDto>();
        }

        public async Task Consume(ConsumeContext<FuturePriceDto> context)
        {
            var priceData = context.Message;
            _logger.LogInformation("Received price data: {Symbol} - {Price}", priceData.Symbol, priceData.Price);
            
            _priceBuffer[priceData.Symbol] = priceData;

       
            if (_priceBuffer.Count >= 2)
            {
               
                var prices = _priceBuffer.Values.Take(2).ToList();

                
                if (prices[0].Symbol != prices[1].Symbol)
                {
                    _logger.LogWarning("Symbol mismatch detected. Received symbols: {Symbol1} and {Symbol2}",
                        prices[0].Symbol, prices[1].Symbol);
                    _priceBuffer.Clear();
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var calculationService = scope.ServiceProvider.GetRequiredService<IArbitageCalculation>();
                _logger.LogInformation("Both price data received. Calculating arbitrage for symbol {Symbol}...", prices[0].Symbol);
                try
                {
                    await calculationService.CalculateAndSaveAsync(prices[0], prices[1]);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during arbitrage calculation.");
                }
                finally
                {
                    _priceBuffer.Clear();
                }
            }
            else
            {
                _logger.LogDebug("Waiting for more price data. Current buffer count: {Count}", _priceBuffer.Count);
            }
        }
    }
}
