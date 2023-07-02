var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
