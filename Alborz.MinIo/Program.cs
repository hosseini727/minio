
using Cleint;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IMinioServices, MinioServices>();


builder.Services.AddScoped<IMinioClient, MinioClient>();


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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
