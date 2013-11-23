using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Logging
{
    public class Log
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime? Created { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}][{1}] {2}", this.Type, this.Created, this.Message);
        }
    }
}
