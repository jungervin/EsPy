using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Jedi
{
    public class SignaturesRequest : Request
    {
        public SignaturesRequest(Script script) : base("signature", script)
        {

        }
    }
}
