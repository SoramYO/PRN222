//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;

//namespace ConsoleApp1
//{
//    class TCPServer
//    {
//        private static List<TcpClient> clients = new List<TcpClient>();
//        private static readonly object lockObj = new object();

//        static void Main()
//        {
//            TcpListener server = null;
//            try
//            {
//                int port = 12345;
//                server = new TcpListener(IPAddress.Any, port);
//                server.Start();
//                Console.WriteLine($"Server started on port {port}. Waiting for connection...");

//                while (true)
//                {
//                    TcpClient client = server.AcceptTcpClient();
//                    lock (lockObj)
//                    {
//                        clients.Add(client);
//                    }
//                    Console.WriteLine("Client connected!");
//                    Thread clientThread = new Thread(HandleClient);
//                    clientThread.Start(client);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error: {ex.Message}");
//            }
//            finally
//            {
//                server?.Stop();
//            }
//        }

//        private static void HandleClient(object obj)
//        {
//            TcpClient client = (TcpClient)obj;
//            NetworkStream stream = client.GetStream();

//            try
//            {
//                while (true)
//                {
//                    byte[] buffer = new byte[1024];
//                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
//                    if (bytesRead == 0) break;

//                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                    Console.WriteLine($"Client: {message}");
//                    BroadcastMessage(message, client);
//                }
//            }
//            catch
//            {
//                Console.WriteLine("Connection closed.");
//            }
//            finally
//            {
//                lock (lockObj)
//                {
//                    clients.Remove(client);
//                }
//                client.Close();
//            }
//        }

//        private static void BroadcastMessage(string message, TcpClient sender)
//        {
//            byte[] data = Encoding.UTF8.GetBytes(message);
//            lock (lockObj)
//            {
//                foreach (var client in clients)
//                {
//                    if (client != sender)
//                    {
//                        NetworkStream stream = client.GetStream();
//                        stream.Write(data, 0, data.Length);
//                    }
//                }
//            }
//        }
//    }
//}
