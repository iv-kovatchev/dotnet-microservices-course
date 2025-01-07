using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

builder.Services.AddHostedService<MessageBusSubscriber>();

//gRPC
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

//Message Bus - RabbitMQ Listening for event
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{ 
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CommandService", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommandService v1"));
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();
