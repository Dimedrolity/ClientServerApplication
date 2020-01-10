﻿using System;

 namespace Client
{
    public static class ClientStarter
    {
        public static void Main(string[] args)
        {
            var client = new TcpServerClient("127.0.0.1", 10000);

            while (true)
            {
                Console.Write("Клиент: ");
                var message = Console.ReadLine();
                
                if(message == "exit" || message == "")
                    break;
                
                client.SendToServer(message);
                var receivedMessage = client.ReceiveFromServer();
                Console.WriteLine($"Клиент: получил от сервера сообщение - {receivedMessage}");
            }

            client.LeaveFromServer();
            Console.WriteLine("Клиент: закрываю свой поток и удаляю себя (экземпляр клиента)");
        }
    }
}