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

namespace Client_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ip = IpAddressTextBox.Text.Trim();
            string portText = PortTextBox.Text.Trim();
            string username = UerNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portText))
            {
                MessageBox.Show("Please enter both IP and Port.");
                return;
            }

            if (!IsValidIP(ip))
            {
                MessageBox.Show("Invalid IP address. Please enter a valid IP.");
                return;
            }
            if (!int.TryParse(portText, out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("Invalid port number. Please enter a valid port.");
                return;
            }
            ChatWindow chatWindow = new ChatWindow(ip, port, username);
            chatWindow.Show();
            chatWindow.ConnectSever();
            this.Close();
        }
        private bool IsValidIP(string ip)
        {
            var segments = ip.Split('.');
            if (segments.Length != 4) return false;

            foreach (var segment in segments)
            {
                if (!int.TryParse(segment, out int num) || num < 0 || num > 255)
                {
                    return false;
                }
            }
            return true;
        }
    }
}