using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsPy.Units
{
    public class PySerial : SerialPort
    {
        

        public const byte CTRL_A = 1; //CTRL-A        -- on a blank line, enter raw REPL mode
        public const byte CTRL_B = 2; //CTRL-B        -- on a blank line, enter normal REPL mode
        public const byte CTRL_C = 3; //CTRL-C        -- interrupt a running program
        public const byte CTRL_D = 4; //CTRL-D        -- on a blank line, do a soft reset of the board
        public const byte CTRL_E = 5; //CTRL-E        -- on a blank line, enter paste mode

        public void EnterRawMode()
        {
            this.Write(CTRL_A);
        }

        public void LeaveRawMode()
        {
            this.Write(CTRL_B);
        }

        public void Interrupt()
        {
            this.Write(CTRL_C);
        }

        public void SoftReset()
        {
            this.Write(CTRL_D);
        }

        public void PasteMode()
        {
            this.Write(CTRL_E);  
        }

        public void Paste(string paste)
        {
            this.PasteMode();
            byte[] buff = Encoding.UTF8.GetBytes(paste.Replace("\r", ""));
            this.Write(buff, 0, buff.Length);
            this.SoftReset();
        }

        public void Clean()
        {
            this.Interrupt();
            this.Interrupt();
            string rec = "";
            do
            {
                Thread.Sleep(50);
                rec = this.ReadExisting();
            } while (rec != "");
        }

        public PySerialEvents.BeginFileProgressEvent BeginFileProgress = null;
        public PySerialEvents.FileProgressEvent FileProgress = null;
        public PySerialEvents.EndFileProgress EndFileProgress = null;

        public ResultStatus Upload(string fname, byte[] buff)
        {
            if (this.BeginFileProgress != null)
                this.BeginFileProgress(this, new FileProgressEventArgs(fname, buff.Length, 0));

            this.WriteLine($"f=open('{fname}', 'wb')");
            this.ReadLine();

            int count = 0;
            string rec = "";
            for (int i = 0; i < buff.Length;)
            {
                int len = Math.Min(32, buff.Length - i);
                this.WriteLine($"f.write({ BinaryString.BinToString(buff, i, len)})");
                i += len;

                rec = this.ReadLine();
                rec = this.ReadLine();
                int c = 0;
                if (int.TryParse(rec, out c))
                {
                    count += c;
                }
                else
                {
                    string error = this.ReadAllLines();
                    if (this.EndFileProgress != null)
                        this.EndFileProgress(this, new FileProgressEventArgs(fname, buff.Length, 0));

                    return new ResultStatus(ResultStatus.Statuses.Error, error);
                }

                if (this.FileProgress != null)
                    this.FileProgress(this, new FileProgressEventArgs(fname, buff.Length, count));

                //Globals.Terminal.UpdateTerminal("", count.ToString(">>> # bytes written"));
                //Application.DoEvents();
            }

            this.WriteLine("f.flush()");
            rec = this.ReadLine();
            this.WriteLine("f.close()");
            rec = this.ReadLine();

            if (this.EndFileProgress != null)
                this.EndFileProgress(this, new FileProgressEventArgs(fname, buff.Length, count));

            return new ResultStatus(ResultStatus.Statuses.Success, count.ToString("# bytes written"));
        }

        private const int BUFF_LEN = 64;
        public ResultStatus Upload2(string fname, byte[] buff)
        {
            if (this.BeginFileProgress != null)
                this.BeginFileProgress(this, new FileProgressEventArgs(fname, buff.Length, 0));

            this.Interrupt();
            this.Interrupt();
            this.PasteMode();
            this.WriteLine("def u(d):");
            this.WriteLine(" l=0");
            this.WriteLine(" for i in range(0, len(d), 2):");
            this.WriteLine("  l+=f.write(bytes([int(d[i: i + 2], 16)]))");
            this.WriteLine(" print(l)");
            this.SoftReset();

            string res = this.ReadAllLines();
            this.WriteLine($"f=open('{fname}', 'wb')");
            res = this.ReadLine();
            int written = 0;
            int length = 0;
            for (int i = 0; i < buff.Length;)
            {
                int len = Math.Min(BUFF_LEN, buff.Length - i);
                this.WriteLine($"u('{BitConverter.ToString(buff, i, len).Replace("-", "")}')");
                i += len;
                res = this.ReadLine();
                res = this.ReadLine();
                
                if (int.TryParse(res, out length))
                {
                    written += length;
                }
                else
                {
                    break;
                }
            }
            this.WriteLine("f.flush()");
            this.WriteLine("f.close()");
            // this.LeaveRawMode();
            
            res = this.ReadAllLines();
            Console.WriteLine(length);
            return null;
        }

        public ResultStatus Download(string fname)
        {
            ResultStatus r = this.Stat(fname);
            if (r.Result == ResultStatus.Statuses.Error)
                return r;

            PyFile f = r.Data as PyFile;

            if (this.BeginFileProgress != null)
                this.BeginFileProgress(this, new FileProgressEventArgs(fname, f.FileSize, 0));

            this.WriteLine($"f=open('{fname}', 'rb')");
            this.ReadLine();

            string rec = "";
            string res = "";
            int count = 0;
            do
            {
                this.WriteLine("f.read(32)");
                try
                {
                    rec = this.ReadLine();

                    if (rec != ">>> f.read(32)")
                    {
                        Thread.Sleep(50);
                        rec = this.ReadExisting();
                        return new ResultStatus(ResultStatus.Statuses.Error, "Unknow line!\r\n" + rec);
                    }

                    // b'' 
                    rec = this.ReadLine();
                    rec = rec.Remove(0, 2);
                    rec = rec.Remove(rec.Length - 1, 1);
                    res += rec;

                    // a little cheat :) 
                    count += 32;
                    if (count > f.FileSize)
                        count = f.FileSize;

                    if (FileProgress != null)
                        this.FileProgress(this, new FileProgressEventArgs(fname, f.FileSize, count));
                }
                catch
                {
                    break;
                }

            } while (rec != "");

            this.WriteLine("f.close()");
            rec = this.ReadLine();
            byte[] buff = BinaryString.Text2Bin(res);
            if (this.EndFileProgress != null)
                this.EndFileProgress(this, new FileProgressEventArgs(f.FileName, f.FileSize, buff.Length));

            if (buff.Length == f.FileSize)
                return new ResultStatus(ResultStatus.Statuses.Success, buff);

            return new ResultStatus(ResultStatus.Statuses.Error, "File Sizes are differents!");
        }

        public string ReadAllLines()
        {
            string rec = "";
            string res = "";
            do
            {
                Thread.Sleep(50);
                rec = this.ReadExisting();
                res += rec;

            } while (rec != "");

            return res;
        }

        public ResultStatus Exec(List<string> lines)
        {
            this.ReadExisting();

            string l = "";
            foreach (string line in lines)
            {
                this.WriteLine(line);
                l = this.ReadLine();
            }

            DateTime t = DateTime.Now;

            string res = "";
            while (true)
            {
                string rec = this.ReadExisting();
                if (rec == "")
                {
                    if ((DateTime.Now - t).TotalMilliseconds > this.ReadTimeout)
                    {
                        return new ResultStatus(ResultStatus.Statuses.Error, "Timeout!");
                    }

                    if (res.EndsWith(PROMPT))
                        break;

                    Thread.Sleep(10);
                }
                else
                {
                    res += rec;
                    t = DateTime.Now;
                }
            }

            string[] items = res.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> result = new List<string>();
            for (int i = 0; i < items.Length - 1; i++)
            {
                if (!items[i].StartsWith("..."))
                    result.Add(items[i]);
            }

            if (res.StartsWith("Traceback") && res.ToLower().Contains("error"))
            {
                return new ResultStatus(ResultStatus.Statuses.Error, result);
            }

            return new ResultStatus(ResultStatus.Statuses.Success, result);
        }

        public ResultStatus Cwd()
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add("os.getcwd()");
            return this.Exec(prog);
        }

        public ResultStatus Stat(string fname)
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add($"os.stat('{fname}')");
            ResultStatus res = this.Exec(prog);
            if (res.Result == ResultStatus.Statuses.Error)
                return res;

            string[] items = res.ToString().Replace("(", "").Replace(")", "").Split(',');
            res.Data = new PyFile(fname, int.Parse(items[0].Trim()), int.Parse(items[6].Trim()));
            return res;
        }

        public ResultStatus Cd(string path)
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add($"os.chdir('{path}')");
            return this.Exec(prog);
        }

        public ResultStatus MkDir(string dir)
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add($"os.mkdir('{dir}')");
            return this.Exec(prog);
        }

        public ResultStatus Rename(string old_name, string new_name)
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add($"os.rename('{old_name}','{new_name}')");
            return this.Exec(prog);
        }

        public ResultStatus Remove(string fname)
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add($"os.remove('{fname}')");
            return this.Exec(prog);
        }

        public ResultStatus RmDir(string dir)
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add($"os.rmdir('{dir}')");
            return this.Exec(prog);
        }
        public ResultStatus Ls()
        {
            List<string> prog = new List<string>();
            prog.Add("import os");
            prog.Add("l=os.listdir()");
            prog.Add("for f in l:");
            prog.Add(" s=os.stat(f)");
            prog.Add(" print('{0},{1},{2}'.format(f, s[0], s[6]))\r\n\b");
            ResultStatus res = this.Exec(prog);
            if (res.Result == ResultStatus.Statuses.Success)
            {
                List<PyFile> files = new List<PyFile>();
                foreach (string line in (res.Data as List<string>))
                {
                    string[] items = line.Split(',');
                    files.Add(new PyFile(items[0], int.Parse(items[1]), int.Parse(items[2])));
                }
                res.Data = files.OrderBy(k => k.FileName).OrderBy(k => k.FileType).ToList();
            }
            return res;
        }

        public string Dir(string path)
        {
            try
            {
                if (path == "")
                    this.WriteLine("dir()");
                else this.WriteLine($"dir({path})");
                //Thread.Sleep(1000);
                //string rec = this.ReadLine();
                //rec = this.ReadLine();
                //if (rec.StartsWith("["))
                //{
                //    return rec.Substring(1, rec.Length - 2).Replace("'", "").Replace(",", "");
                //}
                //else this.ReadAllLines();

            }
            catch
            { }
            finally
            {
                //this.Sync(false);
                //this.ReadAllLines();
            }
            return null;
        }

    }
}
