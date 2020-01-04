﻿//NetChat Server by J4n1X
//Yes, it looks terrible, leave me alone!

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private IPAddress serverIP;

        [STAThread]
        static void Main(string[] args)
        {
            //just init the Server
            Server server = new Server();
            if(args.Length == 0)
            {
                Console.WriteLine("No args defined, starting Server with IP-prompt...");
                server.StartServer();
            }
            else
            {
                server.StartServer(args[0]);
            }
            server.ClientAdd();
        }

        public void StartServer(string temp = "")
        {
            if (temp == "")
            {
                Console.Write("Enter Server-IP:");
                temp = Console.ReadLine();
                serverIP = IPAddress.Parse("127.0.0.1");
            }
            else
            {
                serverIP = IPAddress.Parse(temp);
            }
            serverSocket = new TcpListener(serverIP,8888);
            serverSocket.Start();
            Console.WriteLine("Server successfully loaded!");
        }
        public void StopServer()
        {
            //This is currently unused, perhaps i'll use it later...
        }
        public void ClientAdd()
        {
            while (true)
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

        //Currently unused
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }

    public class clientHandler
    {
        TcpClient clientSocket;
        string clNo;
        IDictionary<string, TcpClient> clientsList;
        public static IDictionary<string, Thread> threadList = new Dictionary<string, Thread>();

        public void StartClientThread(TcpClient inClientSocket, string clineNo, IDictionary<string, TcpClient> cList)
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
