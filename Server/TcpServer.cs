using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class TcpServer
    {
        private readonly TcpListener server;
        private const int BytesCountForMessageLength = 4;

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
                new Thread(() => { ReceiveFromAndSendMessageTo(client); }).Start();
            }
        }

        private void ReceiveFromAndSendMessageTo(TcpClient client)
        {
            var clientStream = client.GetStream();
            while (true)
            {
                string message;
                try
                {
                    message = ReceiveMessageFrom(clientStream);
                    Console.WriteLine(
                        $"Сервер: от клиента {client.Client.RemoteEndPoint} получено сообщение - {message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Сервер: клиент {client.Client.RemoteEndPoint} оборвал связь");
                    break;
                }

                message = message.GetSymbolsSeparatedBy(' '); // обработка сообщения

                SendTo(clientStream, message);

                Console.WriteLine(
                    $"Сервер: отправил клиенту {client.Client.RemoteEndPoint} сообщение - {message}");
            }
        }

        private string ReceiveMessageFrom(NetworkStream clientStream)
        {
            int messageLength = GetMessageLength(clientStream);
            var receivedMessage = new byte[messageLength];

            var bytesCountToRead = receivedMessage.Length;            
            
            var readBytes = 0;
            while (bytesCountToRead > 0)
            {
                readBytes += clientStream.Read(receivedMessage, readBytes, bytesCountToRead);
                bytesCountToRead = receivedMessage.Length - readBytes;
            }

            return Encoding.UTF8.GetString(receivedMessage);
        }

        private int GetMessageLength(NetworkStream clientStream)
        {
            var receivedMessageLength = new byte[BytesCountForMessageLength];
            clientStream.Read(receivedMessageLength, 0, BytesCountForMessageLength);
            return receivedMessageLength.ToIntLittleEndian();
        }

        private void SendTo(NetworkStream clientStream, string message)
        {
            byte[] lengthAndMessage = GetBytesOfLengthAndMessage(message);
            clientStream.Write(lengthAndMessage, 0, lengthAndMessage.Length);
        }

        private byte[] GetBytesOfLengthAndMessage(string message)
        {
            var sentMessage = Encoding.UTF8.GetBytes(message);
            var sentMessageLength = BitConverter.GetBytes(sentMessage.Length);
            return sentMessageLength.Concat(sentMessage).ToArray();
        }
    }
}