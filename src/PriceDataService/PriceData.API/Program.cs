using Microsoft.EntityFrameworkCore;
using PriceData.Application.Interfaces;
using PriceData.Application.Jobs;
using PriceData.Infrastructure.Data;
using PriceData.Infrastructure.Repositories;
using Quartz;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.WriteTo.Console();
});


builder.Services.AddDbContext<PriceDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection")));

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
         .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

//// Migration (opsiyonel)
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<PriceDataContext>();
//    db.Database.Migrate();
//}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();