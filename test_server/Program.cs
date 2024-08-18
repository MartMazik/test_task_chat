using Microsoft.AspNetCore.WebSockets;
using TestServer.Scripts.Common;
using TestServer.Scripts.Common.Logs;
using TestServer.Scripts.Repositories;
using TestServer.Scripts.Service;
using TestServer.Scripts.Sockets;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
AppSettings.Initialize(builder.Configuration);

ConfigurationServices(builder.Services);
ConfigurationSwagger(builder.Services);
ConfigurationLogger();

var app = builder.Build();

ConfigurationWebSocket(app);
ConfigurationMiddleware(app);

app.Run();

void ConfigurationServices(IServiceCollection services)
{
    services.AddSingleton<PostgresDal>();
    services.AddSingleton<IMessageService, MessageService>();
    services.AddSingleton<IMessageRepository, MessagePsqlRepository>();
    services.AddSingleton<IMessageSocketService, MessageSocketService>();
    services.AddControllers();

    services.AddWebSockets(options => { options.KeepAliveInterval = TimeSpan.FromSeconds(120); });

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

void ConfigurationSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void ConfigurationLogger()
{
    Logger.AddDestination(new ConsoleLogDestination());
    Logger.AddDestination(new DateFileLogDestination(AppSettings.LogsDirectory));
}

void ConfigurationWebSocket(WebApplication webApplication)
{
    webApplication.UseWebSockets();
    webApplication.Map("/ws", async context =>
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            await WebSocketHandler.HandleWebSocket(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    });
}

void ConfigurationMiddleware(WebApplication webApplication)
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();

    webApplication.UseCors("AllowAllOrigins");

    webApplication.UseRouting();
    webApplication.MapControllers();

    webApplication.UseExceptionHandler(app =>
    {
        app.Run(async context =>
        {
            Logger.Error($"Unhandled exception: {context.Features.Get<IExceptionHandlerFeature>()?.Error.Message}", "Exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
        });
    });
}
