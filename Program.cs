using NetChat;
using System;
using System.Net.Sockets;

namespace Chat
{
    class Program
    {

        static void Main(string[] args)
        {
            String start = Console.ReadLine();

            if (start == "1")
            {
                Server server = new Server();
                server.serverLoader();
            }
            else
            {
                ClientForm form = new ClientForm();
                form.ShowDialog();
                form.Close();
            }
        }
    }
}
