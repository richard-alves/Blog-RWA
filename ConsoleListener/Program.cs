using Microsoft.AspNetCore.SignalR.Client;
using System;

var uri = "http://localhost:5000/notificationhub";

var connection = new HubConnectionBuilder()
    .WithUrl(uri, options =>
    {
        options.HttpMessageHandlerFactory = (handler) =>
        {
            if (handler is HttpClientHandler clientHandler)
            {
                clientHandler.ServerCertificateCustomValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => { return true; };
            }
            return handler;
        };
    })
    .Build();

try
{
    await connection.StartAsync();
    Console.WriteLine("Conectado ao WebSocket. Aguardando notificações...");

    connection.On<string, string>("ReceiveMessage", (user, message) =>
    {
        Console.WriteLine($"Novo post de {user}: {message}");
    });

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao conectar ao WebSocket: {ex.Message}");
}
