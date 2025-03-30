using Carter;
using PriceData.Application.Interfaces;
using PriceData.Application.Jobs;

namespace PriceData.API.Endpoints;

public class PriceDataEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/pricedata/last/{symbol}", async (HttpContext context, string symbol, IFuturePriceRepository repository) =>
        {
            var lastPrice = await repository.GetLastPriceAsync(symbol, DateTime.UtcNow);
            if (lastPrice == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Price not found");
            }
            else
            {
                await context.Response.WriteAsJsonAsync(lastPrice);
            }
        });

    }
}
