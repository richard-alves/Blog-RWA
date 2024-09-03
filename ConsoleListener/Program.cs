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
    Console.WriteLine("Connected to the Hub!");

    connection.On<string, string>("ReceiveMessage", (user, message) =>
    {
        Console.WriteLine($"Message received from {user}: {message}");
    });

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
