using Arbitrage.Application.Interfaces;
using Carter;

namespace Arbitrage.API.Endpoints;

public class ArbitageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/arbitrage/last", async (HttpContext context, IArbitrageRepository repository) =>
        {
            var result = await repository.GetLastResultAsync(DateTime.UtcNow);
            if (result == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Arbitrage result not found");
            }
            else
            {
                await context.Response.WriteAsJsonAsync(result);
            }
        });
    }
}
