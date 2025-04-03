var builder = WebApplication.CreateBuilder(args);

// Serilog 
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.WriteTo.Console();
});


builder.Services.AddDbContext<ArbitrageContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArbitrageConnection")));


builder.Services.AddScoped<IArbitrageRepository, ArbitrageRepository>();
builder.Services.AddScoped<IArbitageCalculation, ArbitrageCalculationService>();

//RabbitMQ
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddHostedService<PriceDataConsumer>();

builder.Services.AddCarter();

builder.Services.AddScoped<PriceDataConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMassTransit(x =>
{
    // Consumer’ı ekliyoruz
    x.AddConsumer<PriceDataConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        // Endpoint’i konfigüre ediyoruz
        cfg.ReceiveEndpoint(builder.Configuration["RabbitMQ:PriceDataQueue"] ?? "PriceDataQueue", e =>
        {
            e.ConfigureConsumer<PriceDataConsumer>(context);
        });
    });
});


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
