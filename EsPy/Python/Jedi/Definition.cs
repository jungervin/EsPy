using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Python.Jedi
{
    public class Definition : BaseDefinition
    {
        public string desc_with_module = null;
        public DefinedNames defined_names = null;
        public bool? is_definition = null;

        public string Text
        {
            get
            {
                return name;
            }
        }
    }
}
