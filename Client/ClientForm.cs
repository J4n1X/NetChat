//NetChat ClientForm by J4n1X
//Yes, it looks terrible, leave me alone!

using NetChat.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NetChat
{
    public partial class ClientForm : Form
    {
        private List<string> _Log = new List<string>();
        public List<string> Log { get { return _Log; } set { _Log = value; } }

        private string[] commands = { "/sys/disconnect", "/sys/msg", "/sys/file" };

        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default;
        Thread ctThread;
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
                    clientSocket.Connect(IPAddress.Parse(serverIPBox.Text), 8888);
                    serverStream = clientSocket.GetStream();
                    readData = "Connected to Chat Server ...";
                    _Log.Add(readData + "IP = " + serverIPBox.Text);
                    msg();
                }
                catch (SocketException ex)
                {
                    //Returns the Connection Error as a Message Box (Text grabbed from Resource File)
                    _Log.Add(Convert.ToString(ex));
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
            var prompt = MessageBox.Show(Resources.ServerStartPrompt, Resources.MboxQuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (prompt == DialogResult.Yes)
            {
                IPAddressPrompt ipPrompt = new IPAddressPrompt();
                ipPrompt.ShowDialog();
                ipPrompt.Dispose();
                _Log.Add("Started Server from Client");

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
                send(commands[0]);
                DisposeObj();
            }
            catch (Exception)
            {
                _Log.Add(Resources.ClosingError + "(They are not important, the Form is closing anyway!)");
            }
            var Prompt = MessageBox.Show(Resources.SaveLogPrompt, Resources.MboxQuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Prompt == DialogResult.Yes)
            {
                FileStream file = File.Open("NetChat.log", FileMode.OpenOrCreate);
                byte[] dateLine = Encoding.UTF8.GetBytes($"--------------------\n{DateTime.Now.ToString("dd.MM.yyyy HH:mm")}\n--------------------\n");
                file.Write(dateLine, 0, dateLine.Length);
                foreach (string str in _Log) 
                {
                    byte[] strBytes = Encoding.UTF8.GetBytes(str);
                    file.Write(strBytes, 0, strBytes.Length);
                }
                file.Close();
            }
        }




        //Catch ALL non-handeled Errors
        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(Resources.ApplicationThreadError + ": " + e.Exception.Message, Resources.ErrorTitle);
            _Log.Add("FATAL:" + Resources.ApplicationThreadError + ": " + e.Exception);
            Application.Exit();
            throw e.Exception;

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(Resources.CurrentDomainError + ": " + (e.ExceptionObject as Exception).Message, Resources.ErrorTitle);
            _Log.Add("FATAL: " + Resources.CurrentDomainError + ": " + (e.ExceptionObject as Exception));
            Application.Exit();
            throw (e.ExceptionObject as Exception);

        }



        //Non-Event-Methods
        private void send(string Message)
        {
            if ((string.IsNullOrEmpty(Message)) || (Message == null) || serverStream == default) { return; }
            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(Message + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            msgBox.Text = "";
            _Log.Add("Client wrote: " + Message);
        }
        private void getMessage()
        {
            serverStream = clientSocket.GetStream();
            while (true)
            {
                int buffSize = 0;
                byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                buffSize = clientSocket.ReceiveBufferSize;
                //This is because if the Read fails, the program exits because of an unhandeled exception
                try { serverStream.Read(inStream, 0, buffSize); } catch { continue; }
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
            if(ctThread != default)
                ctThread.Abort();
            clientSocket.Close();
        }

    }

}
