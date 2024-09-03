using Microsoft.AspNetCore.SignalR.Client;

public class SignalRClientService : IHostedService
{
    private readonly ILogger<SignalRClientService> _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private HubConnection _connection;

    public SignalRClientService(ILogger<SignalRClientService> logger, IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(OnStarted);
        return Task.CompletedTask;
    }

    private async void OnStarted()
    {
        try
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/notificationhub")
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"########################## Notificação na API: Novo post de {user}: {message} ##########################");
            });

            await _connection.StartAsync();
            _logger.LogInformation("Conectado ao WebSocket. Aguardando notificações...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao conectar ao WebSocket.");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}
