using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ByteStreamUtils
{
    public class Streamer
    {
        private readonly NetworkStream stream;

        private static int BytesCountForMessageLength = 4;

        public Streamer(NetworkStream stream)
        {
            this.stream = stream;
        }

        public void Write(string message)
        {
            byte[] lengthAndMessage = GetBytesOfLengthAndMessage(message);
            stream.Write(lengthAndMessage, 0, lengthAndMessage.Length);
        }

        private byte[] GetBytesOfLengthAndMessage(string message)
        {
            var sentMessage = Encoding.UTF8.GetBytes(message);
            var sentMessageLength = BitConverter.GetBytes(sentMessage.Length);
            return sentMessageLength.Concat(sentMessage).ToArray();
        }

        public string ReadMessage()
        {
            var messageBuffer = CreateBuffer();
            ReadMessageBytesTo(messageBuffer);
            var response = Encoding.UTF8.GetString(messageBuffer);

            return response;
        }

        private byte[] CreateBuffer()
        {
            var receivedMessageLength = new byte[BytesCountForMessageLength];
            stream.Read(receivedMessageLength, 0, BytesCountForMessageLength);
            int messageLength = receivedMessageLength.ToIntLittleEndian();
            var receivedMessage = new byte[messageLength];
            return receivedMessage;
        }

        private void ReadMessageBytesTo(byte[] receivedMessage)
        {
            var bytesCountToRead = receivedMessage.Length;

            var readBytes = 0;
            while (bytesCountToRead > 0)
            {
                readBytes += stream.Read(receivedMessage, readBytes, bytesCountToRead);
                bytesCountToRead = receivedMessage.Length - readBytes;
            }
        }

        public void CloseStream()
        {
            stream.Close();
        }
    }
}