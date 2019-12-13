using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetChat
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
        ConfigSetter.settings settings = new ConfigSetter.settings();

            globalSettings.prginfo = settings.prgInfo;
            globalSettings.serverIP = settings.serverIP;
        }
    }
    static class globalSettings
    {
        public static string prginfo { get; set; }
        public static string serverIP { get; set; }
    }
}
