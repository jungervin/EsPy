using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Utility
{
    public class Device
    {
        public string Name = "";
        public string Cmd = "";
        public string Comment = "";

        public Device(string name, string cmd, string comment)
        {
            this.Name = name;
            this.Cmd = cmd;
            this.Comment = comment;
        }

        public static Device FromString(string text)
        {
            text = text.Trim();
            if(text.Length != 0 && !text.Trim().StartsWith("#") && !text.Trim().StartsWith("/"))
            {
                string[] items = text.Split(';');

                if (items.Length == 2)
                {
                    return new Device(items[0].Trim(), items[1].Trim(), "");
                }
                else if(items.Length == 3)
                {
                    return new Device(items[0].Trim(), items[1].Trim(), items[2].Trim());
                }
            }
            return null;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }


    public class Devices : List<Device>
    {
        public static Devices LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                Devices l = new Devices();

                string[] lines = File.ReadAllLines(filename);
                foreach (string line in lines)
                {
                    if (line.Trim().Length > 0)
                    {
                        Device d = Device.FromString(line);
                        if (d != null)
                            l.Add(d);
                    }
                }
                return l;
            }
            else
            {
                Helpers.ErrorBox($"Could not load the devices from {filename}");
            }
            return null;
        }

    }
}
