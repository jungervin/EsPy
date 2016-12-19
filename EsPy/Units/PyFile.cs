using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Units
{
    public class PyFile
    {
        public string FileName = "";
        public int FileType = 0;
        public int FileSize = 0;

        public PyFile(string file_name, int file_type, int file_size)
        {
            this.FileName = file_name;
            this.FileType = file_type;
            this.FileSize = file_size;
        }

        public bool IsFile
        { get { return (this.FileType & 0x8000) == 0x8000; } }

        public bool IsDir
        { get { return (this.FileType & 0x4000) == 0x4000; } }
    }
}
