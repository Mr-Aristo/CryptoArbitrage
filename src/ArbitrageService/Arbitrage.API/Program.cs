var builder = WebApplication.CreateBuilder(args);

// Serilog 
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.WriteTo.Console();
});


builder.Services.AddDbContext<ArbitrageContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArbitrageConnection")));


builder.Services.AddScoped<IArbitrageRepository, ArbitrageRepository>();
builder.Services.AddScoped<IArbitageCalculation,ArbitrageCalculationService>();

//RabbitMQ
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddHostedService<PriceDataConsumer>();

builder.Services.AddCarter();

builder.Services.AddHostedService<PriceDataConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



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
