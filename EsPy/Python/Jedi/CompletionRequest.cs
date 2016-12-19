using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Jedi
{
    public class CompletionRequest : Request
    {
        
        public CompletionRequest(Script script) : base("completion", script)
        { }
    }
}
