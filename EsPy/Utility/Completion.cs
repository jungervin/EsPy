using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Utility
{
    public class Completion
    {
        public string Module = "";
        public string Complete = "";
        public string Name = "";
        public string[] Params = null;
        public string Type = "";
        public string Doc = "";

        public Completion()
        { }

        public Completion(string module, string complete, string name, string[] args, string type, string doc)
        {
            this.Module = module;
            this.Complete = complete;
            this.Name = name;
            this.Params = args;
            this.Type = type;
            this.Doc = doc;

        }

        public override string ToString()
        {
            return this.Module + " " + this.Name;
            //return base.ToString();
        }

    }
}
