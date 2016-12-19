using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python
{
    public class PyRequest
    {
        public string Module = "";
        public string Method = "";
        public PyRequest(string module, string method)
        {
            this.Module = module;
            this.Method = method;
        }
    }
}
