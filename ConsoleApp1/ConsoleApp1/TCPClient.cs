//using System;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;

//namespace ConsoleApp1
//{
//    class TCPClient
//    {
//        static void Main()
//        {
//            try
//            {
//                string serverIP = "192.168.100.165"; // Replace with server's IP address
//                int port = 12345;

//                TcpClient client = new TcpClient(serverIP, port);
//                Console.WriteLine("Connected to server!");
//                NetworkStream stream = client.GetStream();

//                Thread receiveThread = new Thread(() =>
//                {
//                    while (true)
//                    {
//                        try
//                        {
//                            byte[] buffer = new byte[1024];
//                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
//                            if (bytesRead == 0) break;

//                            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                            Console.WriteLine($"Server: {message}");
//                        }
//                        catch
//                        {
//                            Console.WriteLine("Connection closed.");
//                            break;
//                        }
//                    }
//                });
//                receiveThread.Start();

//                while (true)
//                {
//                    string message = Console.ReadLine();
//                    byte[] data = Encoding.UTF8.GetBytes(message);
//                    stream.Write(data, 0, data.Length);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error: {ex.Message}");
//            }
//        }
//    }
//}
