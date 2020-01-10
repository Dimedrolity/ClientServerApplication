using System;

namespace Server
{
    public static class ServerStarter
    {
        public static void Main(string[] args)
        {
            var server = new TcpServer("127.0.0.1", 10000);
            Console.WriteLine("Сервер: сейчас я запущусь и буду ждать клиентов");
            server.StartAndAcceptClients();
        }
    }
}