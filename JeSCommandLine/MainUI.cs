using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JeSCommandLine
{
    public partial class MainUI : Form
    {
        private int counter;
        public MainUI()
        {
            InitializeComponent();
            this.dockpanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            OpenNewCommandLineTerminal();
        }

        private void OpenNewCommandLineTerminal()
        {
            counter ++;
            JeSTerminal terminal = new JeSTerminal(@"cmd","");
            terminal.Text = "Command Line #" + counter;
            terminal.Show(dockpanel);
        }

      
   

        private void commandLineToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenNewCommandLineTerminal();
        }
    }
}
