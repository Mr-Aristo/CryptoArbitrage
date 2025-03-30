using Carter;
using Microsoft.EntityFrameworkCore;
using PriceData.API.Middlewares;
using PriceData.Application.Interfaces;
using PriceData.Application.Jobs;
using PriceData.Infrastructure.Data;
using PriceData.Infrastructure.Repositories;
using Quartz;
using RabbitMQ.Client;
using Serilog;

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
builder.Services.AddScoped<FuturesPriceService>();

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

// RabbitMQ Connection
var factory = new ConnectionFactory()
{
    HostName = builder.Configuration["RabbitMQ:Host"],
    UserName = builder.Configuration["RabbitMQ:UserName"],
    Password = builder.Configuration["RabbitMQ:Password"]
};

builder.Services.AddSingleton(factory);


var app = builder.Build();

//// Migration (opsiyonel)
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<PriceDataContext>();
//    db.Database.Migrate();
//}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();
app.UseAuthorization();
app.MapCarter();
app.Run();