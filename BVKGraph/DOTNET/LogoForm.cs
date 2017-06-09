using System;
using System.Windows.Forms;

namespace BVKGraph
{
    public partial class LogoForm : Form
    {
        private int _clock;
        public LogoForm()
        {
            InitializeComponent();
        }

        private void timerLogo_Tick(object sender, EventArgs e)
        {
            if (_clock == 2)
            {
                Close();
                Dispose();
            }
            else _clock++;
        }

        private void timerOpacity_Tick(object sender, EventArgs e)
        {
            Opacity += .01;
        }

        private void LogoForm_Load(object sender, EventArgs e)
        {
            labVersion.Text = "ver: "+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
