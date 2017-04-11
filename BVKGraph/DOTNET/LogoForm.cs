using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DOTNET
{
    public partial class LogoForm : Form
    {
        private int clock = 0;
        public LogoForm()
        {
            InitializeComponent();
        }

        private void timerLogo_Tick(object sender, EventArgs e)
        {
            if (clock == 2)
            {
                this.Close();
                this.Dispose();
            }
            else clock++;
        }

        private void timerOpacity_Tick(object sender, EventArgs e)
        {
            this.Opacity += .01;
        }

        private void LogoForm_Load(object sender, EventArgs e)
        {
            labVersion.Text = "ver: "+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
