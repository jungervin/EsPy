using EsPy.Units;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Utility
{
    public static class Helpers
    {
        public static bool PortIsOpen(string port_name)
        {
            System.IO.Ports.SerialPort p = new System.IO.Ports.SerialPort();
            p.PortName = port_name;

            try
            {
                p.Open();
                p.Close();
                return false;
            }
            catch
            {
                return true;
            }
            finally
            {
                if (p != null)
                {
                    p.Dispose();
                    p = null;
                }
            }
        }

        public static string GetPythonPath()
        {
            string[] paths = Environment.GetEnvironmentVariable("PATH").Split(new char[] { ';' });
            foreach (string p in paths)
            {
                string path = Path.Combine(p, "python.exe");
                if (File.Exists(path))
                {
                    return path;
                }
            }
            return "";
        }

        public static DialogResult QuestionBox(string question, string title = "Question")
        {
            return MessageBox.Show(question, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult WarningBox(string warning, string title = "Warning")
        {
            return MessageBox.Show(warning, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ErrorBox(Exception e)
        {
            return MessageBox.Show(e.Message + "\r\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ErrorBox(string msg)
        {
            return MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ErrorBox(int exitcode, string msg)
        {
            return ErrorBox($"ExitCode: {exitcode}\r\n\r\n{msg}");
        }

        public static DialogResult ErrorBox(ResultStatus op)
        {
            if (op != null)
            {
                return MessageBox.Show(op.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                return MessageBox.Show("Data is null at ErrorBox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
