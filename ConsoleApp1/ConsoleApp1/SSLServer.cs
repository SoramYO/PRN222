using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecureTcpServer
{
    public class Message
    {
        public string Type { get; set; }  // "chat", "system", "connect"
        public string Content { get; set; }
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ConnectedClient
    {
        public string Username { get; set; }
        public SslStream Stream { get; set; }
    }

    public class Server
    {
        private readonly X509Certificate2 _serverCertificate;
        private readonly TcpListener _listener;
        private readonly ConcurrentDictionary<string, ConnectedClient> _clients;

        public Server(string certPath, string certPassword)
        {
            _serverCertificate = new X509Certificate2(certPath, certPassword);
            _listener = new TcpListener(IPAddress.Any, 8888);
            _clients = new ConcurrentDictionary<string, ConnectedClient>();
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Server started on port 8888...");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Waiting for connections...");

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] New connection attempt from {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            ConnectedClient connectedClient = null;

            try
            {
                using (SslStream sslStream = new SslStream(
                    client.GetStream(),
                    false,
                    ValidateClientCertificate))
                {
                    await sslStream.AuthenticateAsServerAsync(_serverCertificate);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] SSL authentication successful for client {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                    byte[] buffer = new byte[4096];
                    while (true)
                    {
                        int bytesRead = await sslStream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Client connection closed");
                            break;
                        }

                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        var clientMessage = JsonSerializer.Deserialize<Message>(receivedData);

                        if (clientMessage.Type == "connect")
                        {
                            // Handle new connection
                            connectedClient = new ConnectedClient
                            {
                                Username = clientMessage.Sender,
                                Stream = sslStream
                            };
                            _clients.TryAdd(clientMessage.Sender, connectedClient);

                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] New user '{clientMessage.Sender}' connected");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Current active users: {_clients.Count}");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Active users list:");
                            foreach (var user in _clients.Keys)
                            {
                                Console.WriteLine($"  - {user}");
                            }

                            // Broadcast connection message
                            var connectMessage = new Message
                            {
                                Type = "connect",
                                Content = $"{clientMessage.Sender} has joined the chat",
                                Sender = "Server",
                                Timestamp = DateTime.UtcNow
                            };
                            await BroadcastMessageAsync(connectMessage);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Broadcast: {connectMessage.Content}");

                            // Send welcome message to new client
                            var welcomeMessage = new Message
                            {
                                Type = "system",
                                Content = $"Welcome {clientMessage.Sender}! There are {_clients.Count} users online.",
                                Sender = "Server",
                                Timestamp = DateTime.UtcNow
                            };
                            await SendMessageAsync(sslStream, welcomeMessage);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sent welcome message to {clientMessage.Sender}");
                        }
                        else
                        {
                            // Broadcast regular chat message
                            await BroadcastMessageAsync(clientMessage);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Message from {clientMessage.Sender}: {clientMessage.Content}");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Message broadcast to {_clients.Count} users");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error handling client: {ex.Message}");
            }
            finally
            {
                if (connectedClient != null)
                {
                    _clients.TryRemove(connectedClient.Username, out _);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] User '{connectedClient.Username}' disconnected");
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Remaining active users: {_clients.Count}");

                    // Broadcast disconnect message
                    var disconnectMessage = new Message
                    {
                        Type = "connect",
                        Content = $"{connectedClient.Username} has left the chat",
                        Sender = "Server",
                        Timestamp = DateTime.UtcNow
                    };
                    await BroadcastMessageAsync(disconnectMessage);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Broadcast: {disconnectMessage.Content}");

                    if (_clients.Count > 0)
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Remaining users list:");
                        foreach (var user in _clients.Keys)
                        {
                            Console.WriteLine($"  - {user}");
                        }
                    }
                }
                client.Close();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Client connection closed and cleaned up");
            }
        }

        private async Task BroadcastMessageAsync(Message message)
        {
            var failedClients = new List<string>();
            foreach (var client in _clients)
            {
                try
                {
                    await SendMessageAsync(client.Value.Stream, message);
                }
                catch
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to send message to user: {client.Key}");
                    failedClients.Add(client.Key);
                }
            }

            // Clean up failed clients
            foreach (var failedClient in failedClients)
            {
                if (_clients.TryRemove(failedClient, out _))
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Removed disconnected user: {failedClient}");
                }
            }
        }

        private async Task SendMessageAsync(SslStream sslStream, Message message)
        {
            string jsonMessage = JsonSerializer.Serialize(message);
            byte[] messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
            await sslStream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        private bool ValidateClientCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Initializing chat server...");
            var server = new Server("C:/Users/ngoxu/Downloads/server.pfx", "ngoson02");
            await server.StartAsync();
        }
    }
}