using EsPy.Forms;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Utility
{
    public class FileFormats : List<FileFormat>
    {
        public string DefaultExt = "";

        public string Filters
        {
            get
            {
                string known = "All known files|";
                string ksep = "";
                string filters = "";
                string sep = "";
                foreach (FileFormat format in this)
                {
                    filters += sep + format.Filter;                    
                    sep = "|";
                    if (format != FileFormat.All)
                    {
                        known += ksep + format.Filter.Split('|')[1];
                        ksep = ";";
                    }
                }
                return known + "|" + filters;
            }
        }

        public FileFormat Find(string extension)
        {
            extension = "*" + extension;
            foreach (FileFormat ff in this)
            {
                foreach (string f in ff.Filters)
                    if (f == extension)
                        return ff;
                //if (ff.Filters.Contains("*" + extension))
                //    return ff;
            }
            return null;
        }

        public int FindIndex(string extension)
        {
            for(int i = 0; i < this.Count; i++)
            {
                if (this[i].DefaultExt == extension )
                    return i + 2;
            }
            return 0;
        }
    }
}
