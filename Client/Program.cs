using NetChat;
using System;

//This Program is the Main Menu, for now
namespace Chat
{
    class Program
    {
        public string ServerIPAddr;
        [STAThread]
        static void Main()
        {
            String start = Console.ReadLine();
            ClientForm form = new ClientForm();
            form.ShowDialog();
            form.Close();
        }
    }
}
