using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

 if (builder.Environment.IsProduction()) {
     var connectionString = configuration.GetConnectionString("PlatformsConn");
     if (string.IsNullOrEmpty(connectionString))
     {
         throw new Exception("SQL Server connection string is not configured.");
     }
     else
     {
        Console.WriteLine("--> Using SqlServer Db");
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
     }
 }
 else {
     Console.WriteLine("--> Using InMem Db");
     builder.Services.AddDbContext<AppDbContext>(opt =>
         opt.UseInMemoryDatabase("InMem"));
 }

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

//gRPC
builder.Services.AddGrpc(options => {
    options.EnableDetailedErrors = true;
});

//Message Bus - RabbitMQ
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
});

// Logging Environment
Console.WriteLine($"--> Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"--> CommandService Endpoint {configuration["CommandService"]}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context => {
   await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto")); 
});

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();