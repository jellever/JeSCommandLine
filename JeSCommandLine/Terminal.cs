using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace JeSCommandLine
{
    public partial class JeSTerminal : DockContent
    {
        private ProcessWrapper process;
        private bool warnOnActivity;
        private Icon oldIcon;

        public JeSTerminal(string prog, string args)
        {
            process = new ProcessWrapper(prog, args);
            InitializeComponent();
            this.terminalControl1.Command += terminalControl1_Command;
            this.process.OutputReceived += process_OutputReceived;
            this.process.ErrorReceived += process_ErrorReceived;
            this.warnOnActivity = false;
        }



        void process_ErrorReceived(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    terminalControl1.WriteFormattedText(new List<TextBlock>()
                     {
                         new TextBlock(){Text = "[!] ", Color = TerminalControl.red},
                         new TextBlock(){Text = str + Environment.NewLine, Color = terminalControl1.ForeColor}
                     });
                }));
            }
        }

        void process_OutputReceived(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    terminalControl1.WriteText(str + Environment.NewLine);
                }));
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (this.oldIcon != null)
            {
                this.Icon = oldIcon;
                this.oldIcon = null;
            }
        }

        void terminalControl1_Command(string command)
        {
            process.ExecuteCommand(command);
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            process.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                process.ErrorReceived -= process_ErrorReceived;
                process.OutputReceived -= process_OutputReceived;
                process.Dispose();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
