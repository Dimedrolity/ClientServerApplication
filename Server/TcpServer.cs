using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ByteStreamUtils;

namespace Server
{
    public class TcpServer
    {
        private readonly TcpListener server;
        private Streamer streamer;


        public TcpServer(string ip, int port)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void StartAndAcceptClients()
        {
            server.Start();
            AcceptClients();
        }

        public void AcceptClients()
        {
            while (true)
            {
                var client = server.AcceptTcpClient();
                Console.WriteLine($"Сервер: клиент {client.Client.RemoteEndPoint} присоединился ко мне!");
                new Thread(() =>
                {
                    var stream = client.GetStream();
                    streamer = new Streamer(stream);
                    ReceiveAndSendMessageTo(client);
                }).Start();
            }
        }

        private void ReceiveAndSendMessageTo(TcpClient client)
        {
            while (true)
            {
                string message;
                try
                {
                    message = streamer.ReadMessage();
                    Console.WriteLine(
                        $"Сервер: от клиента {client.Client.RemoteEndPoint} получено сообщение - {message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Сервер: клиент {client.Client.RemoteEndPoint} оборвал связь");
                    break;
                }
                
                // обработка сообщения перед отправкой клиенту
                message = GetCharsSeparatedBySpace(message);    
                
                streamer.Write(message);

                Console.WriteLine(
                    $"Сервер: отправил клиенту {client.Client.RemoteEndPoint} сообщение - {message}");
            }
        }
        
        public string GetCharsSeparatedBySpace(string message)
        {
            var charArray = message.ToCharArray();
            return string.Join(' ', charArray);
        }
    }
}