using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Jedi
{
    public class Script
    {
        public int Line = 0;
        public int Column = 0;
        public string Source = "";

        public Script(string source, int line, int column) 
        {
            this.Source = source;
            this.Line = line;
            this.Column = column;
        }
    }
}
