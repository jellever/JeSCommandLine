using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JeSCommandLine
{
    class TerminalControl : RichTextBox
    {
        public static Color red = Color.FromArgb(211, 46, 42);
        public static Color darkblue = Color.FromArgb(11, 68, 198);//Color.FromArgb(59, 87, 126);
        public static Color yellow = Color.FromArgb(177, 156, 0);
        public static Color lightblue = Color.FromArgb(0, 187, 187);
        public static Color GreenColor = Color.FromArgb(9, 248, 0);
        public static Color purple = Color.FromArgb(84, 84, 255);

        private string shellIdentifier;
        private const string shellSeperator = " > ";
        private bool showTextFirst;
        private bool useShellPrefix;

        public delegate void CommandDelegate(string command);
        public event CommandDelegate Command;


        public TerminalControl()
        {
            this.Text = "";
            showTextFirst = true;
            shellIdentifier = "";
            useShellPrefix = false;
            this.DetectUrls = false;
        }



        public void SetShellIdentifier(string id)
        {
            string last = this.Lines.LastOrDefault();
            if (last != null && LastLineBeginsWithShellPRefix())
            {
                RemoveLastLineShellPrefix();

            }
            this.shellIdentifier = id;
            AppendShellPrefix();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (showTextFirst && useShellPrefix)
            {
                this.Clear();
                this.AppendShellPrefix();
            }
        }

        protected void OnCommand(string command)
        {
            if (Command != null)
                Command(command);
        }

        protected void AppendText(string text, Color? color)
        {
            if (color != null)
                this.SelectionColor = color.Value;
            else
                this.SelectionColor = this.ForeColor;
            this.SelectedText = text;
            this.SelectionColor = this.ForeColor;
        }

        protected void AppendTextEnd(string text, Color color)
        {
            this.SelectionStart = this.TextLength;
            this.SelectionLength = 0;
            this.SelectionColor = color;
            this.SelectedText = text;
            this.SelectionColor = this.ForeColor;
        }

        private string GetShellPrefix()
        {
            if (useShellPrefix)
                return shellIdentifier + shellSeperator;
            return "";
        }



        public bool LastLineEndsWithShellPrefix()
        {
            string lastl = this.Lines.LastOrDefault();
            string shellPrefix = GetShellPrefix();
            return lastl.EndsWith(shellPrefix);
        }

        public bool LastLineBeginsWithShellPRefix()
        {
            string fullcommand = this.Lines.LastOrDefault();
            string shellPrefix = GetShellPrefix();
            return !String.IsNullOrEmpty(fullcommand) && fullcommand.StartsWith(shellPrefix);
        }

        public void RemoveLastLineShellPrefix()
        {
            RemoveLastLine();
            return;
            string shellPrefix = GetShellPrefix();
            string lastl = this.Lines.LastOrDefault();
            int i = this.TextLength - shellPrefix.Length;
            this.Select(i, shellPrefix.Length);
            this.SelectedText = "";
        }

        public void RemoveLastLine()
        {
            string lastl = this.Lines.LastOrDefault();
            int i = this.TextLength - lastl.Length;
            this.Select(i, lastl.Length);
            this.SelectedText = "";
        }

        public void AppendShellPrefix()
        {
            if (!useShellPrefix)
                return;
            string shellp = GetShellPrefix();
            this.AppendTextEnd(shellp, Color.White);
        }
        public void AppendShellPrefixNewLine()
        {
            string shellp = GetShellPrefix() + Environment.NewLine;
            this.AppendTextEnd(shellp, Color.White);
        }

        public void WriteText(string text)
        {
            this.WriteText(text, this.ForeColor);
        }

        private void Redraw()
        {
            this.Invalidate();
            this.Update();
        }

        public void WriteFormattedText(List<TextBlock> text)
        {
            string last = this.Lines.LastOrDefault();
            if (last != null && (LastLineBeginsWithShellPRefix()) && text.Count > 0)
            {/*
                int i = this.GetFirstCharIndexFromLine(this.Lines.Count() - 1);
                this.Select(i, 0);
                foreach (TextBlock block in text)
                {
                    this.AppendText(block.Text , block.Color);
                }
              
                this.Select(this.TextLength, 0);
                this.SelectionColor = this.ForeColor;
                */
                RemoveLastLineShellPrefix();
                //Redraw();

                last = this.Lines.LastOrDefault();
                string firstTextBlock = text.First().Text;
                if ((firstTextBlock.StartsWith("\r\n") || firstTextBlock.StartsWith("\n")) && last == "")
                {
                    text.First().Text = firstTextBlock.Substring(2);
                }


                bool endsWithNewLine = true;
                foreach (TextBlock block in text)
                {
                    if (!String.IsNullOrEmpty(block.Text))
                    {
                        this.AppendText(block.Text, block.Color);
                        endsWithNewLine = block.Text.EndsWith("\n");
                    }
                }
                if (!endsWithNewLine)
                    this.AppendText(Environment.NewLine);
                //Redraw();
                AppendShellPrefix();
                //Redraw();
            }
            else
            {
                foreach (TextBlock block in text)
                {
                    this.AppendText(block.Text, block.Color);
                }
                //AppendShellPrefix();
            }
            this.SelectionColor = this.ForeColor;
        }

        public void WriteText(string text, Color color)
        {
            WriteFormattedText(new List<TextBlock>() { new TextBlock() { Color = color, Text = text } });
        }

        protected bool ShouldSkipKeyPress(KeyEventArgs e, out bool resetColor)
        {
            resetColor = true;
            int line = GetLineFromCharIndex(this.SelectionStart);
            int endLine = GetLineFromCharIndex(this.TextLength);
            bool isControlC = e.Control && e.KeyCode == Keys.C;
            bool isArrow = e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Right || e.KeyCode == Keys.Down;
            string shellPrefix = GetShellPrefix();

            if (line != endLine)
            {
                resetColor = false;
                if (!isControlC && !isArrow)
                {
                    return true;
                }
            }
            else
            {
                int lineBeginCharIndex = GetFirstCharIndexFromLine(endLine);
                int commandStartCharIndex = lineBeginCharIndex + shellPrefix.Length;
                if (this.SelectionStart < commandStartCharIndex && !isControlC && !isArrow)
                {
                    resetColor = false;
                    return true;
                }
                else if (e.KeyCode == Keys.Back && this.SelectionStart - 1 < commandStartCharIndex)
                {
                    resetColor = true;
                    return true;
                }
                else if (isControlC || isArrow)
                {
                    resetColor = false;
                }
            }
            return false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool resetColor;
            if (ShouldSkipKeyPress(e, out resetColor))
            {
                e.SuppressKeyPress = true;
            }
            if (resetColor)
                this.SelectionColor = this.ForeColor;


            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                string command = this.Lines.LastOrDefault();
                if (command != null)
                {
                    if (useShellPrefix)
                    {
                        string shellPrefix = GetShellPrefix();
                        if (command.Length > shellPrefix.Length)
                            command = command.Remove(0, shellPrefix.Length);
                        AppendTextEnd(Environment.NewLine, ForeColor);
                        AppendShellPrefix();
                    }
                    else
                    {
                        RemoveLastLine();
                    }
                    OnCommand(command);
                }
            }
        }


    }
}
