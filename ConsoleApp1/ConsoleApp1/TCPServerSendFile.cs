//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Linq;

//namespace ConsoleApp1
//{
//    class TCPServerSendFile
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

//                // Start a thread for server input
//                Thread serverInputThread = new Thread(HandleServerInput);
//                serverInputThread.Start();

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

//        private static void HandleServerInput()
//        {
//            while (true)
//            {
//                string message = Console.ReadLine();
//                if (!string.IsNullOrEmpty(message))
//                {
//                    Console.WriteLine($"Server: {message}");
//                    BroadcastMessage($"Server: {message}", null);
//                }
//            }
//        }

//        private static void HandleClient(object obj)
//        {
//            TcpClient client = (TcpClient)obj;
//            NetworkStream stream = client.GetStream();

//            try
//            {
//                byte[] buffer = new byte[8192];
//                while (true)
//                {
//                    using (MemoryStream ms = new MemoryStream())
//                    {
//                        int bytesRead;
//                        do
//                        {
//                            bytesRead = stream.Read(buffer, 0, buffer.Length);
//                            if (bytesRead == 0) break;
//                            ms.Write(buffer, 0, bytesRead);
//                        } while (stream.DataAvailable);

//                        if (bytesRead == 0) break;

//                        byte[] receivedData = ms.ToArray();
//                        string content = Encoding.UTF8.GetString(receivedData);

//                        try
//                        {
//                            // Try to parse as numbers
//                            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
//                            if (lines.All(line => int.TryParse(line, out _)))
//                            {
//                                // This is a file with numbers
//                                string fileName = $"mat_{DateTime.Now:yyyyMMddHHmmss}.txt";

//                                // 1. Notify client that file is received
//                                string notifyMessage = "File received successfully!";
//                                byte[] notifyData = Encoding.UTF8.GetBytes(notifyMessage);
//                                stream.Write(notifyData, 0, notifyData.Length);
//                                Console.WriteLine($"Server: {notifyMessage}");

//                                // 2. Save the file
//                                File.WriteAllText(fileName, content);
//                                Console.WriteLine($"File saved as: {fileName}");

//                                // 3. Broadcast to other clients
//                                BroadcastMessage($"Server received file: {fileName}", client);
//                            }
//                            else
//                            {
//                                // Handle as regular chat message
//                                Console.WriteLine($"Client: {content}");
//                                BroadcastMessage($"Client: {content}", client);
//                            }
//                        }
//                        catch
//                        {
//                            // If parsing fails, treat as regular message
//                            Console.WriteLine($"Client: {content}");
//                            BroadcastMessage($"Client: {content}", client);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Connection closed: {ex.Message}");
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
//                    if ((sender == null || client != sender) && client.Connected)
//                    {
//                        try
//                        {
//                            NetworkStream stream = client.GetStream();
//                            stream.Write(data, 0, data.Length);
//                        }
//                        catch (Exception)
//                        {
//                            clients.Remove(client);
//                        }
//                    }
//                }
//            }
//        }

//        private static bool ContainsBinaryData(string text)
//        {
//            foreach (char c in text)
//            {
//                if (c == '\0' || (c < 32 && c != '\n' && c != '\r' && c != '\t'))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//    }
//}
