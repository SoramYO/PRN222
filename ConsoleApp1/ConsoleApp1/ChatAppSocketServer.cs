//using System.Net.Sockets;
//using System.Net;
//using System.Text;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using System;

//class ChatServer
//{
//    private static TcpListener _listener;
//    private static List<TcpClient> _clients = new List<TcpClient>();
//    private static readonly string LogFilePath = "chat_log.txt"; // Path to the log file

//    static void Main()
//    {
//        Console.WriteLine("Starting server...");
//        _listener = new TcpListener(IPAddress.Any, 5000); // Lắng nghe tất cả địa chỉ IP
//        _listener.Start();
//        Console.WriteLine("Server started on port 5000.");

//        while (true)
//        {
//            TcpClient client = _listener.AcceptTcpClient();
//            _clients.Add(client);
//            Console.WriteLine("Client connected.");
//            Thread clientThread = new Thread(() => HandleClient(client));
//            clientThread.Start();
//        }
//    }

//    private static void HandleClient(TcpClient client)
//    {
//        NetworkStream stream = client.GetStream();
//        StreamReader reader = new StreamReader(stream, Encoding.UTF8);

//        try
//        {
//            while (true)
//            {
//                // Đọc tin nhắn từ client
//                string message = reader.ReadLine();
//                if (message == null) // Client đóng kết nối
//                {
//                    Console.WriteLine("Client disconnected.");
//                    break;
//                }

//                Console.WriteLine($"Received: {message}");
//                LogMessage(message); // Log the message to a file
//                Broadcast(message, client); // Phát tin nhắn tới tất cả client
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Client disconnected.");
//        }
//        finally
//        {
//            _clients.Remove(client);
//            client.Close();
//        }
//    }

//    private static void Broadcast(string message, TcpClient excludeClient)
//    {
//        byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
//        foreach (var client in _clients)
//        {
//            try
//            {
//                if (client == excludeClient) continue;
//                client.GetStream().Write(buffer, 0, buffer.Length);
//            }
//            catch (Exception)
//            {
//                Console.WriteLine("Error sending message.");
//            }
//        }
//    }

//    private static void LogMessage(string message)
//    {
//        try
//        {
//            lock (LogFilePath) // Ensure thread safety
//            {
//                using (StreamWriter writer = new StreamWriter(LogFilePath, true, Encoding.UTF8))
//                {
//                    writer.WriteLine($"{DateTime.Now}: {message}");
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error logging message: {ex.Message}");
//        }
//    }
//}
