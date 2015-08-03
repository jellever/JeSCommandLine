using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace JeSCommandLine
{
    class ProcessWrapper : IDisposable
    {
        private string file;
        private string args;
        private Process process;
        private bool running;
        private Thread stdoutThread;
        private Thread stderrThread;

        public delegate void StringDelegate(string str);
        public event StringDelegate OutputReceived;
        public event StringDelegate ErrorReceived;

        public ProcessWrapper(string file, string args)
        {
            this.file = file;
            this.args = args;
        }

        public void Start()
        {
            Kill();
            process = new Process();
            process.StartInfo.FileName = file;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            //process.EnableRaisingEvents = true;
            process.Exited += process_Exited;
            running = process.Start();

            StartStdThreads();
            process.StandardInput.AutoFlush = true;
            //process.OutputDataReceived += process_OutputDataReceived;

            //process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutputReceived != null)
                OutputReceived(e.Data);
        }

        private void StartStdThreads()
        {
            stdoutThread = new Thread(new ThreadStart(ReadStandardOutputWorker));
            stdoutThread.IsBackground = true;
            stdoutThread.Start();

            stderrThread = new Thread(new ThreadStart(ReadStandardErrorWorker));
            stderrThread.IsBackground = true;
            stderrThread.Start();
        }


        public void Kill()
        {
            if (process != null)
            {
                process.Kill();
                process.Exited -= process_Exited;
                running = false;
            }
        }

        private void ReadStandardOutputWorker()
        {
            try
            {
                // use Stream.Read() instead of Peek because Read blocks until a char is available
                char firstChar;
                while (running && (firstChar = (char)process.StandardOutput.Read()) > -1)
                    ReadStream(process.StandardOutput, firstChar, OutputReceived);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadStandardOutputWorker Error Occured: " + ex.ToString());
            }
        }

        private void ReadStandardErrorWorker()
        {
            try
            {
                // use Stream.Read() instead of Peek because Read blocks until a char is available
                char firstChar;
                while (running && (firstChar = (char)process.StandardError.Read()) > -1)
                    ReadStream(process.StandardError, firstChar, ErrorReceived);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadStandardErrorWorker Error Occured: " + ex.ToString());
            }
        }

        private void ReadStream(StreamReader reader, char firstChar, StringDelegate del)
        {
            StringBuilder buffer = new StringBuilder(256);
            buffer.Append(firstChar);
            lock (this)
            {
                while (reader.Peek() > -1)
                {
                    char ch = (char)reader.Read();
                    buffer.Append(ch);
                }
                if (del != null)
                    del(buffer.ToString());
            }
        }



        public void ExecuteCommand(string str)
        {
            if (running && process != null && !process.HasExited)
            {
                process.StandardInput.WriteLine(str);
            }
        }



        void process_Exited(object sender, EventArgs e)
        {
            running = false;
        }

        public void OnErrorReceived(string err)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived(err);
            }
        }

        public void OnOutputReceived(string str)
        {
            if (OutputReceived != null)
            {
                OutputReceived(str);
            }
        }

        public void Dispose()
        {
            Kill();
        }
    }
}
