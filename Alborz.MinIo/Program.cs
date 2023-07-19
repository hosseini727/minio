
using Cleint;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddSingleton<RabbitMqServices, RabbitMqServices>();
builder.Services.AddSingleton<RabbitMqServices>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var hostName = configuration["RabbitMQ:127.0.0.1"];
    var queueName = configuration["RabbitMQ:ef"];
    return new RabbitMqServices(hostName, queueName);
});

builder.Services.AddScoped<MinioClient, MinioClient>();

builder.Host.UseSerilog((ctx, lc) => lc
    //.WriteTo.Console()
    .WriteTo.File(new JsonFormatter(),
        "important-logs.json",
        restrictedToMinimumLevel: LogEventLevel.Error)
    // Add a log file that will be replaced by a new log file each day
    .WriteTo.File("all-daily-.logs",
        rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
    // Set default minimum log level
    .MinimumLevel.Error());

<<<<<<< HEAD
<<<<<<< HEAD
=======


>>>>>>> parent of b08cdd3 (sdad)
//var app = builder.Build();
WebApplication? app = builder.Build();
=======


var app = builder.Build();
//WebApplication? app = builder.Build();
>>>>>>> parent of bd34366 (add service rabbit)
var test = app.Services.GetService<RabbitMqServices>();
test.ConsumeMessages();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
public partial class Program { }


