using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace PlutLogRe
{
    public class Runner
    {
        private readonly string _fileName;
        public delegate void InputReceiver(string output);
        private InputReceiver _stdOut;
        private InputReceiver _stdErr;
        private Process p;
        public bool Running { get; private set; }

        public Runner(string fileName, InputReceiver stdOut, InputReceiver stdErr)
        {
            _fileName = fileName;
            _stdOut = stdOut;
            _stdErr = stdErr;
        }

        public void Startup()
        {
            ProcessStartInfo si = new ProcessStartInfo();
            si.CreateNoWindow = true;
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            si.RedirectStandardInput = true;
            si.UseShellExecute = false;
            si.Arguments = "\""+_fileName+"\"";
            si.FileName = Directory.GetCurrentDirectory()+"\\PlutLog.exe";
            p = new Process();
            p.StartInfo = si;
            p.Start();
            p.OutputDataReceived += new DataReceivedEventHandler(
                delegate (object s, DataReceivedEventArgs e)
                {
                    Application.Current.Dispatcher.Invoke(() => _stdOut(e.Data));
                });
            p.BeginOutputReadLine();
            p.ErrorDataReceived += new DataReceivedEventHandler(
                delegate (object s, DataReceivedEventArgs e)
                {
                    Application.Current.Dispatcher.Invoke(() => _stdErr(e.Data));
                });
            p.BeginErrorReadLine();
            Running = true;
            p.Exited += new EventHandler(
                delegate (object s,EventArgs e)
                {
                    Running = false;
                });
        }

        public void Finish()
        {
            p?.Kill();
        }

        public void PutString(string input)
        {
            p.StandardInput.WriteLine(input);
        }
    }
}
