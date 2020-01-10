using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class TcpServerClient
    {
        private readonly TcpClient client;
        private readonly NetworkStream stream;
        private const int BytesCountForMessageLength = 4;
        private byte[] sentMessage;
        private byte[] sentMessageLength;
        private byte[] receivedMessage;
        private byte[] receivedMessageLength;
        
        public TcpServerClient(string ip, int port)
        {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
        }

        public void SendToServer(string message)
        {
            byte[] lengthAndMessage = GetBytesOfLengthAndMessage(message);
            stream.Write(lengthAndMessage, 0, lengthAndMessage.Length);
            
            sentMessage.FillWithZeros();
            sentMessageLength.FillWithZeros();
        }

        private byte[] GetBytesOfLengthAndMessage(string message)
        {
            sentMessage = Encoding.UTF8.GetBytes(message);
            sentMessageLength = BitConverter.GetBytes(sentMessage.Length);
            return sentMessageLength.Concat(sentMessage).ToArray();
        }

        public string ReceiveFromServer()
        {
            AllocateMemoryForBuffer();
            ReceiveMessage();
            var response = Encoding.UTF8.GetString(receivedMessage);
            
            receivedMessage.FillWithZeros();
            receivedMessageLength.FillWithZeros();
            
            return response;
        }

        private void AllocateMemoryForBuffer()
        {
            receivedMessageLength = new byte[BytesCountForMessageLength];
            stream.Read(receivedMessageLength, 0, BytesCountForMessageLength);
            int messageLength = receivedMessageLength.ToIntLittleEndian();
            receivedMessage = new byte[messageLength];
        }
        
        private void ReceiveMessage()
        {
            var bytesCountToRead = receivedMessage.Length;

            var readBytes = 0;
            while (bytesCountToRead > 0)
            {
                readBytes += stream.Read(receivedMessage, readBytes, bytesCountToRead);
                bytesCountToRead = receivedMessage.Length - readBytes;
            }
        }

        public void LeaveFromServer()
        {
            stream.Close();
            client.Close();
        }
    }
}