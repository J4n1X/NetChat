using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;


namespace NetChat
{
    public partial class IPAddressPrompt : Form
    {
        public IPAddressPrompt()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool executeSuccess = false;
            IPAddress IPAddr;
            if (IPAddress.TryParse(inputTextBox.Text, out IPAddr))
            {
                try
                {
                    Process.Start(@"C:\Users\janic\source\repos\Network Adventures\NetChat\Server\bin\Debug\Server.exe", IPAddr.ToString()); //Change this at all costs for Release!
                    executeSuccess = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.Resources.IPAddressPrompt_Error + ex.Message, Properties.Resources.ErrorTitle);
                }
                finally
                {
                    Close();
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
