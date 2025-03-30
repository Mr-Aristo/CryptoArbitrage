using Arbitrage.Application.Interfaces;
using Arbitrage.Application.Services;
using Arbitrage.Infrastructure.Data;
using Arbitrage.Infrastructure.Repositories;
using ArbitrageService.Messaging;
using Carter;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog yap?land?rmas?
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.WriteTo.Console();
});


builder.Services.AddDbContext<ArbitrageContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArbitrageConnection")));


builder.Services.AddScoped<IArbitrageRepository, ArbitrageRepository>();
builder.Services.AddScoped<ArbitrageCalculationService>();

builder.Services.AddCarter();

builder.Services.AddHostedService<PriceDataConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// RabbitMQ Bağlantısı
var factory = new ConnectionFactory()
{
    HostName = builder.Configuration["RabbitMQ:Host"],
    UserName = builder.Configuration["RabbitMQ:UserName"],
    Password = builder.Configuration["RabbitMQ:Password"]
};

builder.Services.AddSingleton(factory);
var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ArbitrageContext>();
//    db.Database.Migrate();
//}


app.UseRouting();
app.UseAuthorization();
app.MapCarter();
app.Run();
