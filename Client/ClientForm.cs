//NetChat ClientForm by J4n1X
//Yes, it looks terrible, leave me alone!

using NetChat.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace NetChat
{
    public partial class ClientForm : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default;
        Thread ctThread;
        List<string> Log = new List<string>();
        string readData;
        public ClientForm()
        {
            InitializeComponent();
            clientSocket.ReceiveTimeout = 1000;
            clientSocket.SendTimeout = 1000;
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }



        //Creates Messaging Thread and Handles Errors while doing so
        private void connectButton_Click(object sender, EventArgs e)
        {
            IPAddress temp;
            if (!IPAddress.TryParse(serverIPBox.Text, out temp))
            {
                MessageBox.Show(Resources.InvalidAddress, Resources.ErrorTitle);
                return;
            }
            else
            {
                try
                {
                    clientSocket.Connect(serverIPBox.Text, 8888);
                    serverStream = clientSocket.GetStream();
                    readData = "Connected to Chat Server ...";
                    Log.Add(readData + "IP = " + serverIPBox.Text);
                    msg();
                }
                catch (SocketException ex)
                {
                    //Returns the Connection Error as a Message Box (Text grabbed from Resource File)
                    Log.Add(Convert.ToString(ex));
                    var msgbox = MessageBox.Show(Resources.ConnectFailedPrompt, Resources.ErrorTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (msgbox == DialogResult.Yes) { MessageBox.Show(ex.Message); }
                    return;
                }
            }
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(userNameTextBox.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            //To avoid Exceptions
            sendButton.Enabled = true;

            ctThread = new Thread(getMessage);
            ctThread.Start();
        }

        private void startServerButton_Click(object sender, EventArgs e)
        {
            var Prompt = MessageBox.Show(Resources.ServerStartPrompt, Resources.MboxQuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Prompt == DialogResult.Yes)
            {
                Process.Start(@"C:\Users\janic\source\repos\Network Adventures\NetChat\Server\bin\Debug\Server.exe", NetChat.Properties.Resources.startServerIP); //Change this at all costs for Release!
                Log.Add("Started Server from Client");
            }
        }
        //these 2 Methods do the same, but in different instances
        private void msgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send(msgBox.Text);
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            send(msgBox.Text);
        }

        //Keep the most recent Text in the top
        private void chatHistory_TextChanged(object sender, EventArgs e)
        {
            if (chatHistory.Visible)
            {
                chatHistory.SelectionStart = chatHistory.TextLength;
                chatHistory.ScrollToCaret();
                chatHistory.DeselectAll();
            }
        }
        
        //Cause artificial Crash
        private void causeLocalDomainCrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        //Clean up and write Log
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisposeObj();
            }
            catch (Exception)
            {
                Log.Add(Resources.ClosingError+"(They are not important, the Form is closing anyway!)");
            }
            var Prompt = MessageBox.Show(Resources.SaveLogPrompt, Resources.MboxQuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Prompt == DialogResult.Yes)
            {
                if (!File.Exists("NetChat.log")) { File.CreateText("NetChat.log"); }
                StreamWriter file = new StreamWriter("NetChat.log",append:true);
                file.WriteLine("--------------------\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + "\n--------------------\n");
                foreach(string str in Log) { file.WriteLine(str); }
                file.Close();

            }
        }




        //Catch ALL non-handeled Errors
        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(Resources.ApplicationThreadError + ": " + e.Exception.Message, Resources.ErrorTitle);
            Log.Add("FATAL:" + Resources.ApplicationThreadError + ": " + e.Exception);
            Application.Exit();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(Resources.CurrentDomainError + ": " + (e.ExceptionObject as Exception).Message, Resources.ErrorTitle);
            Log.Add("FATAL: " + Resources.CurrentDomainError + ": " + (e.ExceptionObject as Exception));
            Application.Exit();
        }

        

        //Non-Event-Methods
        private void send(string Message)
        {
            if ((string.IsNullOrEmpty(Message)) || (Message == null)) { return; }
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Message + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            msgBox.Text = "";
            Log.Add("Client wrote: " + Message);
        }
        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
            }
        }
        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                chatHistory.Text = chatHistory.Text + Environment.NewLine + " >> " + readData;
        }
        private void DisposeObj()
        {
            ctThread.Abort();
            clientSocket.Close();
        }

    }

}
