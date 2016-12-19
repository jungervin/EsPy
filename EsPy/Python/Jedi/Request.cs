using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Jedi
{
    public class Request : PyRequest
    {
        public Script Script = null;
        public Request(string method, Script script) : base("jedi", method)
        {
            this.Script = script;
        }
    }
}
