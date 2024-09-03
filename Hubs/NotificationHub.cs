using Microsoft.AspNetCore.SignalR;

namespace Blog_RWA.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            Console.WriteLine($"Client connected: {Context.ConnectionId}"); // Também imprime na janela de comando
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            _logger.LogInformation($"Received message from {user}: {message}");
            Console.WriteLine($"Received message from {user}: {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task NotifyNewPost(string postTitle)
        {
            await Clients.All.SendAsync("ReceiveNotification", postTitle);
        }
    }
}
