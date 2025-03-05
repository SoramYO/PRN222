using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatAppUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private string serverIp = "10.87.12.85";
        private int serverPort = 5000;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient();
                client.Connect(serverIp, serverPort); // Kết nối tới server
                stream = client.GetStream();
                MessagesList.Items.Add("Đã kết nối tới server!");

                Task.Run(() => ReceiveMessages());
            }
            catch (Exception ex)
            {
                MessagesList.Items.Add($"Lỗi kết nối: {ex.Message}");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    string message = MessageTextBox.Text.Trim();
                    if (!string.IsNullOrEmpty(message))
                    {
                        IPAddress localIp = ((IPEndPoint)client.Client.LocalEndPoint).Address;
                        string formattedIp = localIp.ToString().Replace("::ffff:", "");
                        string currentTime = DateTime.Now.ToString("HH:mm:ss");
                        message = $"[{formattedIp} - {currentTime}]: {message}";

                        byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                        stream.Write(data, 0, data.Length);

                        // Hiển thị tin nhắn trên giao diện
                        MessagesList.Items.Add($"Bạn: {message}");
                        MessageTextBox.Clear();
                    }
                }
                else
                {
                    MessagesList.Items.Add("Không thể gửi tin nhắn: Chưa kết nối tới server.");
                }
            }
            catch (Exception ex)
            {
                MessagesList.Items.Add($"Lỗi khi gửi tin nhắn: {ex.Message}");
            }
        }

        private void ReceiveMessages()
        {
            Thread receiveThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                                Dispatcher.Invoke(() =>
                                {
                                    MessagesList.Items.Add($"{message}");
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MessagesList.Items.Add($"Ngắt kết nối: {ex.Message}");
                        });
                    }
                }
            });
            receiveThread.Start();
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {

                SendButton_Click(this, new RoutedEventArgs());

                e.Handled = true;
            }

        }
    }
}