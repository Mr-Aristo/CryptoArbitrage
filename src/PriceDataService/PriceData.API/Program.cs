
var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.WriteTo.Console();
});


builder.Services.AddDbContext<PriceDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection")));

//Carter for minimal api
builder.Services.AddCarter();

// Dependency Injection
builder.Services.AddScoped<IFuturePriceRepository, FuturePriceRepository>();
builder.Services.AddScoped<IFuturePriceService,FuturesPriceService>();

// Quartz configuration
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("FetchPriceJob");
    q.AddJob<FetchPriceJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
         .ForJob(jobKey)
         .WithIdentity("FetchPriceJob-trigger")
         .WithCronSchedule("0 0/30 * * * ?")
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddEndpointsApiExplorer();

// RabbitMQ 
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();


var app = builder.Build();

//// Migration (opsiyonel)
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<PriceDataContext>();
//    db.Database.Migrate();
//}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();
app.MapCarter();
app.Run();