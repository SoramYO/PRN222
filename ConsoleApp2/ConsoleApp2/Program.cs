//using System;
//using System.IO;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Linq;

//namespace ConsoleApp1
//{
//    class TCPClient
//    {
//        static void Main()
//        {
//            try
//            {
//                string serverIP = "10.87.12.85";
//                int port = 12345;

//                TcpClient client = new TcpClient(serverIP, port);
//                Console.WriteLine("Connected to server!");
//                NetworkStream stream = client.GetStream();

//                // Start a thread to receive messages from the server
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

//                // Main thread for sending messages or files
//                while (true)
//                {
//                    Console.WriteLine("Enter a message to send or type 'sendfile' to send sorted numbers:");
//                    string input = Console.ReadLine();

//                    if (input.ToLower() == "sendfile")
//                    {
//                        SendSortedNumbersFile(stream);
//                    }
//                    else
//                    {
//                        byte[] data = Encoding.UTF8.GetBytes(input);
//                        stream.Write(data, 0, data.Length);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error: {ex.Message}");
//            }
//        }

//        private static void SendSortedNumbersFile(NetworkStream stream)
//        {
//            try
//            {
//                // Generate random numbers and sort them
//                Random random = new Random();
//                int[] numbers = Enumerable.Range(1, 10)
//                    .Select(x => random.Next(1, 100))
//                    .OrderBy(x => x)
//                    .ToArray();

//                // Create file with the correct format
//                string fileName = $"mat_{DateTime.Now:yyyyMMddHHmmss}.txt";
//                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

//                // Write sorted numbers to file
//                File.WriteAllLines(filePath, numbers.Select(n => n.ToString()));
//                Console.WriteLine($"Created file {fileName} with sorted numbers: {string.Join(", ", numbers)}");

//                // Send the file content
//                byte[] fileData = File.ReadAllBytes(filePath);
//                stream.Write(fileData, 0, fileData.Length);
//                stream.Flush();

//                Console.WriteLine("Sorted numbers file sent successfully!");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error sending file: {ex.Message}");
//            }
//        }
//    }
//}