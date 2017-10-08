using EsPy.Components;
using EsPy.Forms;
//using EsPy.Python;
using EsPy.Utility;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy
{
    public class Globals
    {

        //public static PyClient PyClient = null;

        //public static void PyClientStart()
        //{
        //   PyClient = new PyClient(new IPAddress(new byte[] { 127, 0, 0, 1 }),
        //        Properties.Settings.Default.PyServerPort);

        //    try
        //    {
        //        Task t = new Task(() =>
        //        {     
        //            Splash s = new Splash();
        //            s.TopMost = true;
        //            s.Show();
        //            Application.DoEvents();
        //            s.label2.Text = "Connecting...";                  
        //            Application.DoEvents();
        //            Thread.Sleep(2000);
        //            if (Globals.PyClient.Start())
        //            {
        //                s.label2.Text = "Connected!";
        //            }
        //            else
        //            {
        //                s.label2.Text = "Could not connect!";
        //                PyClient = null;
        //            }
        //            Application.DoEvents();
        //            Thread.Sleep(2000);
        //            s.Close();
        //            s.Dispose();

        //            string python = Helpers.GetPythonPath();
        //            if (python == "")
        //            {
        //                string msg = "Python path is not configured or python.exe does not exists!!\r\n\r\nPlease set the %PATH% variable!";
        //                MessageBox.Show( msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
                   
        //        });

        //        t.Start();
        //    }
        //    catch (Exception ee)
        //    {
        //        PyClient = null;
        //    }
        //}

        //public static string PythonExe
        //{ get { return Properties.Settings.Default.PythonExe; } }

        //public static bool PythonExists
        //{
        //    get { return File.Exists(PythonExe); }
        //}

        public static MainForm MainForm
            {get; internal set; }

        public static TerminalForm TerminalForm
        { get { return MainForm.TerminalForm; } }

        public static Terminal Terminal
        { get { return MainForm.TerminalForm.scintilla; }  }

      
    }
}
