using System;
using NUnit.Framework;
using Client;
using Server;

namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void OneClientSendEnglishMessage()
        {
            var client = new TcpServerClient("127.0.0.1", 10000);
            var message = "Redkin Dmitry";
            client.SendToServer(message);

            var expected = GetSymbolsSeparatedBySpace(message);
            var actual = client.ReceiveFromServer();
            client.LeaveFromServer();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OneClientSendRussianMessage()
        {
            var client = new TcpServerClient("127.0.0.1", 10000);
            var message = "Редькин Дмитрий";
            client.SendToServer(message);

            var expected = GetSymbolsSeparatedBySpace(message);
            var actual = client.ReceiveFromServer();
            client.LeaveFromServer();
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void OneClientSendMessageExit()
        {
            var client = new TcpServerClient("127.0.0.1", 10000);
            var message = "";
            client.SendToServer(message);
            
            var expected = "";
            var actual = client.ReceiveFromServer();
            client.LeaveFromServer();
            Assert.AreEqual("", actual);

        }

        [Test]
        public void TwoClientsInOneTime()
        {
            var client = new TcpServerClient("127.0.0.1", 10000);
            var anotherClient = new TcpServerClient("127.0.0.1", 10000);
            
            var clientMessage = "Редькин Дмитрий";
            var anotherClientMessage = "Redkin Dmitry";
            
            client.SendToServer(clientMessage);
            anotherClient.SendToServer(anotherClientMessage);
            
            
            var expectedResponseToClient = GetSymbolsSeparatedBySpace(clientMessage);
            var actualResponseToClient = client.ReceiveFromServer();
            var expectedResponseToAnotherClient = GetSymbolsSeparatedBySpace(anotherClientMessage);
            var actualResponseToAnotherClient = anotherClient.ReceiveFromServer();
            
            client.LeaveFromServer();
            anotherClient.LeaveFromServer();
            Assert.AreEqual(expectedResponseToClient, actualResponseToClient);
            Assert.AreEqual(expectedResponseToAnotherClient, actualResponseToAnotherClient);
        }

        private static string GetSymbolsSeparatedBySpace(string message)
        {
            var charArray = message.ToCharArray();
            return string.Join(' ', charArray);
        }
    }
}