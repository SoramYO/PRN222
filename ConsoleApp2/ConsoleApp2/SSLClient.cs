using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecureTcpClient
{
    public class Message
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Client
    {
        private readonly TcpClient _client;
        private SslStream _sslStream;
        private string _username;

        public Client()
        {
            _client = new TcpClient();
        }

        public async Task ConnectAsync(string username)
        {
            try
            {
                _username = username;
                await _client.ConnectAsync("localhost", 8888);
                _sslStream = new SslStream(
                    _client.GetStream(),
                    false,
                    ValidateServerCertificate);

                await _sslStream.AuthenticateAsClientAsync("localhost");
                Console.WriteLine("Connected to server successfully");

                // Send initial connect message with username
                await SendMessageAsync("", "connect");

                // Start listening for server messages
                _ = ListenForMessagesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                throw;
            }
        }

        private async Task ListenForMessagesAsync()
        {
            byte[] buffer = new byte[4096];
            while (true)
            {
                try
                {
                    int bytesRead = await _sslStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var message = JsonSerializer.Deserialize<Message>(jsonMessage);

                    // Different display format based on message type
                    switch (message.Type)
                    {
                        case "system":
                            Console.WriteLine($"[SYSTEM] {message.Content}");
                            break;
                        case "connect":
                            Console.WriteLine($"[CONNECTION] {message.Content}");
                            break;
                        default:
                            Console.WriteLine($"[{message.Timestamp:HH:mm:ss}] {message.Sender}: {message.Content}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading message: {ex.Message}");
                    break;
                }
            }
        }

        public async Task SendMessageAsync(string messageText, string type = "chat")
        {
            try
            {
                var message = new Message
                {
                    Type = type,
                    Content = messageText,
                    Sender = _username,
                    Timestamp = DateTime.UtcNow
                };

                string jsonMessage = JsonSerializer.Serialize(message);
                byte[] messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
                await _sslStream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw;
            }
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Close()
        {
            _sslStream?.Dispose();
            _client?.Close();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(username))
            {
                Console.Write("Username cannot be empty. Please enter your username: ");
                username = Console.ReadLine();
            }

            var client = new Client();

            try
            {
                await client.ConnectAsync(username);
                Console.WriteLine("Connected to server. Type 'exit' to quit.");

                while (true)
                {
                    string input = Console.ReadLine();
                    if (input?.ToLower() == "exit")
                        break;

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        await client.SendMessageAsync(input);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}