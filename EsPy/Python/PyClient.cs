using EsPy.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Python
{
    public class PyClient
    {
        private TcpClient Client = null;
        private Process Process = null;
        public PyClient(IPAddress address, int port)
        {
            this.Address = address;
            this.Port = port;
        }

        private IPAddress Address
        { get; set; }

        private int Port
        { get; set; }


        private string ScriptPath
        { get { return Path.Combine(Application.StartupPath, "Scripts", "PyHost.py"); } }

        public bool Start()
        {
            if (File.Exists(this.ScriptPath))
            {
                System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo(
                 "python", this.ScriptPath);

                inf.UseShellExecute = false;
#if DEBUG
                inf.CreateNoWindow = false;
#else
                inf.CreateNoWindow = true;
#endif
                this.Process = new System.Diagnostics.Process();
                this.Process.StartInfo = inf;
                try
                {
                    this.Process.Start();
                    Thread.Sleep(1000);
                    
                    int p = 10;
                    while (p-- > 0)
                    {
                        Thread.Sleep(1000);
                        try
                        {
                            JToken res = Globals.PyClient.DoRequest<JToken>("jedi", "hello");
                            if (res != null)
                            {
                                return true;
                            }
                        }
                        catch
                        { }
                    }
                    return p > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }

        public void Stop()
        {
            try
            {
                try
                {
                    if (this.Client != null)
                    {
                        JToken res = this.DoRequest<JToken>("jedi", "bye");
                        //if (res != null)
                        this.Client.Close();
                        this.Client.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Helpers.ErrorBox(e);
                }

                if (this.Process != null)
                {
                    this.Process.Close();
                    this.Process.Dispose();
                }
            }
            catch (Exception ee)
            {
                Helpers.ErrorBox(ee);
            }

        }

        private void Connect()
        {
            try
            {
                if (this.Client == null)
                {
                    this.Client = new TcpClient();
                    // Todo: test size
                    //this.Client.ReceiveBufferSize = 1024;
                    this.Client.ReceiveTimeout = 3000;
                    this.Client.SendTimeout = 3000;
                    this.Client.Connect(this.Address, this.Port);
                }
            }
            catch (Exception ex)
            {
                this.Client = null;
                Globals.PyClient = null;
                Helpers.ErrorBox(ex);
            }
        }


        byte[] InpBuffer = new byte[8192];
        const int MAX_WAIT = 10;

        private string DoRequest(string text)
        {
            this.Connect();

            if (this.Client != null && this.Client.Connected)
            {

                NetworkStream ns = this.Client.GetStream();
                byte[] buff = Encoding.UTF8.GetBytes(text);
                ns.Write(BitConverter.GetBytes(buff.Length), 0, sizeof(Int32));
                ns.Write(buff, 0, buff.Length);

                byte[] rec = new byte[4];
                ns.Read(rec, 0, sizeof(Int32));
                int len = BitConverter.ToInt32(rec, 0);

                if (len > this.InpBuffer.Length)
                {
                    Array.Resize(ref this.InpBuffer, len);
                }
                //byte[] inb = new byte[len];

                //while (this.Client.Available < len)
                //{
                //    Thread.Sleep(10);
                //}

                int count = 0;
                int p = MAX_WAIT;
                while (count < len)
                {

                    if (this.Client.Available == 0)
                    {
                        Thread.Sleep(10);
                        if (p-- <= 0)
                        {
                            throw new TimeoutException("MAX_WAIT Timeout!");
                        }
                    }
                    else
                    {
                        count += ns.Read(this.InpBuffer, count, this.Client.Available);
                        p = MAX_WAIT;
                    }
                }

                //Console.WriteLine($"SOCKET: {count}/{len}");          

                return System.Text.Encoding.UTF8.GetString(this.InpBuffer, 0, len);
            }

            return "";
        }

        public T DoRequest<T>(PyRequest req)
        {
            string json = JsonConvert.SerializeObject(req);
            string res = Globals.PyClient.DoRequest(json);
            return JsonConvert.DeserializeObject<T>(res);
        }

        public T DoRequest<T>(string module, string method)
        {
            PyRequest req = new PyRequest(module, method);
            return DoRequest<T>(req);
        }
    }
}
