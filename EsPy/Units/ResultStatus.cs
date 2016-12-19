using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Units
{
    public class ResultStatus
    {
        public enum Statuses { Success, Error};
        public Statuses Result = Statuses.Success;
        public object Data = "";

        public ResultStatus(Statuses result, object data)
        {
            this.Result = result;
            this.Data = data;
        }

        public override string ToString()
        {
            if (this.Data is string)
            {
                return this.Data.ToString();
            }
            else if(this.Data is IEnumerable<string>)
            {
                return String.Join("\r\n", (this.Data as IEnumerable<string>).ToArray());
            }
            return base.ToString();
        }
    }
}
