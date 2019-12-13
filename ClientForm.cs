using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace NetChat
{
    public partial class ClientForm : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default;
        string readData;
        Thread ctThread;
        public ClientForm()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            readData = "Connected to Chat Server ...";
            msg();
            clientSocket.Connect(serverIPBox.Text, 8888);
            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(userNameTextBox.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            ctThread = new Thread(getMessage);
            ctThread.Start();
        }
        private void chatHistory_TextChanged(object sender, EventArgs e)
        {
            if (chatHistory.Visible)
            {
                chatHistory.SelectionStart = chatHistory.TextLength;
                chatHistory.ScrollToCaret();
                chatHistory.DeselectAll();
            }
        }

        private void msgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                send();
            }
        }
        private void sendButton_Click(object sender, EventArgs e)
        {
            send();
        }
        

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ctThread.Abort();
        }
        private void send()
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msgBox.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            msgBox.Text = "";
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


       
    }
}
