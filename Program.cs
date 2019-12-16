using NetChat;
using System;
using System.Net.Sockets;

//This Program is the Main Menu, for now
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
