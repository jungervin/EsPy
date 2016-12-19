using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsPy.Utility
{
    public class Py
    {

        //public static string Jedi()
        //{
        //    string res = "";

        //    //TcpClient client = new TcpClient()

        //    return res;
        //}
        //private static Process proc = null;
        public static string Run(string cmd, string args, out int exitcode)
        {
            exitcode = 0;
            string res = "";
            try
            {
                System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo(
                   cmd, args);

                inf.RedirectStandardOutput = true;
                inf.RedirectStandardError = true;
                //inf.RedirectStandardInput = true;
                inf.UseShellExecute = false;
                inf.CreateNoWindow = true;

                Process proc = new System.Diagnostics.Process();
                proc.StartInfo = inf;

                proc.Start();

                res = proc.StandardOutput.ReadToEnd();
                res += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
                while (!proc.HasExited)
                {
                    Thread.Sleep(10);
                }

                exitcode = proc.ExitCode;
                proc.Close();
                proc.Dispose();

                return res;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {

            }
        }

        public event EventHandler<string> ErrorReceived;
        public event EventHandler ProcessExited;
        public event EventHandler<string> TextReceived;

        public Py(string cmd, string args)
        {
            this.Process = new Process();
            this.Process.StartInfo.FileName = cmd;
            this.Process.StartInfo.Arguments = args;
            this.Process.StartInfo.RedirectStandardInput = true;
            this.Process.StartInfo.RedirectStandardOutput = true;
            this.Process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            this.Process.StartInfo.RedirectStandardError = true;
            this.Process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            this.Process.StartInfo.UseShellExecute = false;
            this.Process.StartInfo.CreateNoWindow = true;
            this.Process.EnableRaisingEvents = true;

            this.Process.Exited += Process_Exited;
          

        }


        string Data = "";
        //public  Keywords Write(string data)
        //{
        //    this.Data = "";
        //    //this.Process.StandardInput.WriteLine("print(10)");
        //    this.Process.StandardInput.WriteLine(data);
        //    this.Process.StandardInput.Flush();

        //    //List<string> list = new List<string>();
        //    //string res = "";
        //    //string rec = "";
        //    //do
        //    //{
        //    //    rec = this.Process.StandardOutput.ReadLine();

        //    //    if (rec.StartsWith(">>>"))
        //    //        break;

        //    //    if (rec.StartsWith("<<<"))
        //    //    {
        //    //        list.Add(res);
        //    //        res = "";
        //    //        continue;
        //    //    }
        //    //    else
        //    //    {
        //    //        res += rec + "\r\n";
        //    //    }
        //    //} while (true);

        //    return this.ProcessData();
        //}

        public int ExitCode = 0;

        public void Start()
        {
            bool res = this.Process.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            this.ExitCode = this.Process.ExitCode;
        }

        private char[] Buffer = new char[4096];
        private char C0 = '\0';
        private char C1 = '\0';
        private char C2 = '\0';
        private char C3 = '\0';

        //private Keywords ProcessData()
        //{
        //    int n = 0;
        //    Keywords list = new Keywords();
        //    string rec = "";
        //    do
        //    {
        //        n = this.Process.StandardOutput.Read(this.Buffer, 0, this.Buffer.Length - 4);
        //        if (n > 0)
        //        {
        //            for (int i = 0; i < n; i++)
        //            {
        //                this.C0 = this.C1;
        //                this.C1 = this.C2;
        //                this.C2 = this.C3;
        //                this.C3 = this.Buffer[i];

        //                if (this.C0 == '>' && this.C1 == '>' && this.C2 == '>' && this.C3 == ' ')
        //                {
        //                    return list;
        //                }
        //                else if (this.C0 == '<' && this.C1 == '<' && this.C2 == '<' && this.C3 == ' ')
        //                {
        //                    string s = rec.Substring(0, rec.Length - 6);
        //                    i += 2;

        //                    string[] items = s.Split(new string[] { "\r\n" }, StringSplitOptions.None );
        //                    string complete = items[0].Trim();
        //                    string name = items[1].Trim();
        //                    string args = items[2].Trim();
        //                    string type = items[3].Trim();
        //                    string doc = "";
        //                    for (int j = 4; j < items.Length; j++)
        //                    {
        //                        doc += items[j] + "\r\n";
        //                    }

        //                    list.Add(new Keyword(complete, name, args, type, doc));
        //                    rec = "";
        //                }
        //                else
        //                {
        //                    rec += this.C3;
        //                }
        //            }
        //        }
        //    }
        //    while (n > 0);
        //    return list;
        //}    

        private Process Process
        { get; set; }

        
    }
}
