using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.CodeConventions
{
    /// <summary>
    /// Represents a service code convention
    /// </summary>
    public interface ICodeConvention
    {
        string ConvertServiceName(string serviceName);
        string ConvertMethodName(string methodName);
        string ConvertArgumentName(string argumentName);
    }
}
