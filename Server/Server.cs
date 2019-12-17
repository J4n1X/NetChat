using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Chat
{
    class Server
    {
        /*use this link: http://csharp.net-informations.com/communications/csharp-chat-server.htm
         * Summary: This Programs main Thread is always listening to incoming Messages. The first message is always the Username. Messages are the length of the TCPclient Buffer and get terminated by a $. The User gets added to the Userlist and a Thread is created for the User.
         */
        private IPAddress serverIp;
        private TcpListener serverSocket;
        private TcpClient clientSocket = default;
        private static IDictionary<string, TcpClient> ClientsList = new Dictionary<string, TcpClient>();
        private List<string> Log = new List<string>();

        [STAThread]
        static void Main()
        {
            //just init the Server
            Server server = new Server();
            server.StartServer();
            server.ClientAdd();
        }

        public void StartServer()
        {
            Console.Write("Enter Server-IP:");
            var temp = Console.ReadLine();

            if (temp == "")
            {
                serverIp = IPAddress.Parse("127.0.0.1");
            }
            else
            {
                serverIp = IPAddress.Parse(temp);
            }
            serverSocket = new TcpListener(serverIp, 8888);
            serverSocket.Start();
            Console.WriteLine("Server successfully loaded!");
        }
        public void StopServer()
        {
            //This is currently unused, perhaps use later...
        }
        public void ClientAdd()
        {
            while ((true))
            {
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                string dataFromClient;

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                //This gets the Username from the first message.
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                //Now add the User to the list to keep track of him.
                ClientsList.Add(dataFromClient, clientSocket);

                clientHandler clHandler = new clientHandler();
                clHandler.StartClientThread(clientSocket, dataFromClient, ClientsList);
                Broadcast(dataFromClient + " Joined the Server", dataFromClient, false, false);
                Console.WriteLine(dataFromClient + "Joined the Server");
            }
        }

        public static void ClientRemove(string clNo)
        {
            foreach (KeyValuePair<string, Thread> Item in clientHandler.threadList)
            {
                if (clNo == Item.Key)
                {
                    Item.Value.Abort();
                }
            }
        }

        //will handle Client. sysReq will control the passing of a system request to clients in the future.
        public static void Broadcast(string msg, string userName, bool dispName, bool sysReq)
        {
            foreach (KeyValuePair<string, TcpClient> Item in ClientsList)
            {
                TcpClient broadcastSocket = Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBuffer;
                if (dispName) { broadcastBuffer = Encoding.ASCII.GetBytes(userName + " says: " + msg); } else { broadcastBuffer = Encoding.ASCII.GetBytes(msg); }
                broadcastStream.Write(broadcastBuffer, 0, broadcastBuffer.Length);
                broadcastStream.Flush();
            }
        }

    }

    public class clientHandler
    {
#pragma warning disable CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
        TcpClient clientSocket;
#pragma warning restore CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
        string clNo;
#pragma warning disable CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
        IDictionary<string, TcpClient> clientsList;
#pragma warning restore CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
        public static IDictionary<string, Thread> threadList = new Dictionary<string, Thread>();

#pragma warning disable CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
#pragma warning disable CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
        public void StartClientThread(TcpClient inClientSocket, string clineNo, IDictionary<string, TcpClient> cList)
#pragma warning restore CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
#pragma warning restore CS0246 // Der Typ- oder Namespacename "TcpClient" wurde nicht gefunden (möglicherweise fehlt eine using-Direktive oder ein Assemblyverweis).
        {
            clientSocket = inClientSocket;
            clNo = clineNo;
            clientsList = cList;
            Thread clientThread = new Thread(ClientCommunicator);
            //Add ClientName/Thread Pair to Dictionary
            threadList.Add(clNo, clientThread);
            clientThread.Start();
        }

        //This is the Client Thread
        public void ClientCommunicator()
        {
            //inefficient way of storing incoming Bytes, but simple
            byte[] bytesReceived = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient;
            NetworkStream networkStream = clientSocket.GetStream();


            while (true)
            {
                try
                {
                    networkStream.Read(bytesReceived, 0, bytesReceived.Length);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesReceived);
                    dataFromClient = dataFromClient.Split('$')[0];
                    Server.Broadcast(dataFromClient, clNo, true, false);
                    Console.WriteLine("Message from Client - " + clNo + " " + dataFromClient);
                }
                catch (Exception)
                {
                    Console.WriteLine("Client " + clNo + " Disconnecting");
                    Server.ClientRemove(clNo);
                    break;
                }
            }
        }


    }

}
