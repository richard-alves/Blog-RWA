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
                Console.WriteLine($"Notificação interna: Nova mensagem de {user}: {message}");
            });

            await _connection.StartAsync();
            _logger.LogInformation("SignalR Client conectado ao hub.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao conectar ao SignalR Hub.");
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
