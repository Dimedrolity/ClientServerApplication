using System.Net.Sockets;
using ByteStreamUtils;

namespace Client
{
    public class TcpServerClient
    {
        private readonly TcpClient client;
        private readonly Streamer streamer;

        public TcpServerClient(string ip, int port)
        {
            client = new TcpClient(ip, port);
            var stream = client.GetStream();
            streamer = new Streamer(stream);
        }

        public void SendToServer(string message)
        {
            streamer.Write(message);
        }

        public string ReceiveFromServer()
        {
            return streamer.ReadMessage();
        }

        public void LeaveFromServer()
        {
            streamer.CloseStream();
            client.Close();
        }
    }
}