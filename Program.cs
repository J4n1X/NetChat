using NetChat;
using System;
using System.Net.Sockets;

namespace Chat
{
    class Program
    {

        static void Main(string[] args)
        {
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            NetworkStream serverStream = default;
            String start = Console.ReadLine();

            if (start == "1")
            {
                Server server = new Server();
                server.serverLoader();
            }
            else
            {
                MainMenu menu = new MainMenu();
                ClientForm form = new ClientForm();
                menu.ShowDialog();
                form.ShowDialog();
                menu.Close();
                form.Close();
            }

            clientSocket.Close();
        }

    }
}
