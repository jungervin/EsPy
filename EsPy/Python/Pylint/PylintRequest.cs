using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Pylint
{
    public class PylintRequest 
    {
        public string Module = "pylint";
        public string File = ""
        public PylintRequest(string file)
        {
            this.File = file;
        }
    }
}
