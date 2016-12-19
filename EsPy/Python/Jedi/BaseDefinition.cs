using EsPy.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Jedi
{
    public class BaseDefinition : ExListBox.ExListBoxItem
    {
        public int ImageIndex = 1;
        public string module_path = null;
        public string name = null;
        public string type = null;
        public string module_name = null;
        public string in_builtin_module = null;
        public string line = null;
        public string column = null;
        public string doc = null;
        public string docstring = null;
        public string description = null;
        public string full_name = null;
        public string[] parameters = null;

        public virtual string Text
        {
            get
            {
                return name;
            }
        }

        public override string ToString()
        {
            return this.name ?? "no suggestion";
        }
    }
}
