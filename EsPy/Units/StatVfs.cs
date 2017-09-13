using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Units
{
    public class StatVfs
    {


        public int BSize = 0;           //f_bsize – file system block size
        public int FrSize = 0;          //f_frsize – fragment size
        public int SizeInFsUnits = 0;   //f_blocks – size of fs in f_frsize units
        public int BFree = 0;           //f_bfree – number of free blocks
        public int BAvail = 0;          //f_bavail – number of free blocks for unpriviliged users
        public int Files = 0;           //f_files – number of inodes
        public int FFree = 0;           //f_ffree – number of free inodes
        public int FAvail = 0;          //f_favail – number of free inodes for unpriviliged users
        public int Flag = 0;            //f_flag – mount flags
        public int NameMax = 0;         //f_namemax – maximum filename length

        public const float MegaByte = 1024 * 1024;
        private StatVfs()
        {
        }

        public static StatVfs Create(string stat)
        {
            String[] items = TextHelper.Matches(@"(\d+?)[,)]", stat);
            if (items != null)
            {
                StatVfs fs = new StatVfs();
                fs.BSize = int.Parse(items[0]);  
                fs.FrSize = int.Parse(items[1]); 
                fs.SizeInFsUnits = int.Parse(items[2]);
                fs.BFree = int.Parse(items[3]); 
                fs.BAvail = int.Parse(items[4]);
                fs.Files = int.Parse(items[5]);
                fs.FFree = int.Parse(items[6]);
                fs.FAvail = int.Parse(items[7]);
                fs.Flag = int.Parse(items[8]);  
                fs.NameMax = int.Parse(items[9]);
                return fs;
            }
            return null;
        }


        public float TotalBytes
        { get { return this.FrSize * SizeInFsUnits; } }

        public float FreeBytes
        { get { return this.BSize * this.BFree; } }

        public float TotalMegaBytes
        {  get { return this.TotalBytes / MegaByte; } }

        public float FreeMegaBytes
        { get { return this.FreeBytes / MegaByte; } }

        public float Usage
        {
            get
            {
                if (this.TotalBytes != 0)
                    return 1 - (this.FreeBytes / this.TotalBytes);
                return float.NaN;
            }
        }


    }
}
