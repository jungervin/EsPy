using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Units
{
    public interface IPort
    {
        PySerial Port
        { get; set; }
    }
}
