using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client_WPF
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private string _ipAddress;
        private int _port;
        private string _username;
        private readonly TcpClient _client;
        private SslStream _sslStream;
        public ChatWindow(string ipServer, int portServer, string username)
        {
            InitializeComponent();
            _client = new TcpClient();
            _ipAddress = ipServer;
            _port = portServer;
            _username = username;
        }
        public async void ConnectSever()
        {
            try
            {
                await _client.ConnectAsync(_ipAddress, _port);
                _sslStream = new SslStream(
                    _client.GetStream(),
                    false,
                    ValidateServerCertificate);

                await _sslStream.AuthenticateAsClientAsync(_ipAddress);
                ChatHistoryListBox.Text += ("Connected to server successfully\n");
                IpAddressTextBox.Text = _ipAddress;
                PortTextBox.Text = _port.ToString();
                await SendMessageAsync("", "connect");

                _ = ListenForMessagesAsync();
            }
            catch (Exception ex)
            {
                ChatHistoryListBox.Text += ($"Connection error: {ex.Message}\n");
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
                            ChatHistoryListBox.Text += ($"[SYSTEM] {message.Content}\n");
                            break;
                        case "connect":
                            ChatHistoryListBox.Text += ($"[CONNECTION] {message.Content} \n");
                            break;
                        default:
                            ChatHistoryListBox.Text += ($"[{message.Timestamp:HH:mm:ss}] {message.Sender}: {message.Content}\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ChatHistoryListBox.Text += ($"Error reading message: {ex.Message} \n");
                    break;
                }
            }
        }
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                try
                {
                    await SendMessageAsync(message);
                    ChatHistoryListBox.Text += $"You: {message}\n";
                    MessageTextBox.Clear();
                }
                catch (Exception ex)
                {
                    ChatHistoryListBox.Text += $"Error sending message: {ex.Message}\n";
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
                ChatHistoryListBox.Text += ($"Error sending message: {ex.Message} \n");
                throw;
            }
        }
        public class Message
        {
            public string Type { get; set; }
            public string Content { get; set; }
            public string Sender { get; set; }
            public DateTime Timestamp { get; set; }
        }
        private bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }



    }
}
