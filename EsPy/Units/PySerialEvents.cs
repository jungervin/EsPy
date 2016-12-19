using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Units
{
    public class FileProgressEventArgs : EventArgs
    {
        public string FName = "";
        public int Size = 0;
        public int Bytes = 0;

        public FileProgressEventArgs(string fname, int size, int bytes)
        {
            this.FName = fname;
            this.Size = size;
            this.Bytes = bytes;
        }
    }

    public class PySerialEvents
    {
        public delegate void BeginFileProgressEvent(object sender, FileProgressEventArgs e);
        public delegate void FileProgressEvent(object sender, FileProgressEventArgs e);
        public delegate void EndFileProgress(object sender, FileProgressEventArgs e);
    }
}
