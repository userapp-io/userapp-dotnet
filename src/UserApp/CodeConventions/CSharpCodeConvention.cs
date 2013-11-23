using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.CodeConventions
{
    /// <summary>
    /// Adds support for CSharp code convention
    /// </summary>
    public class CSharpCodeConvention : ICodeConvention
    {
        public string ConvertServiceName(string serviceName)
        {
            return ApplyCamelCaseFormat(serviceName);
        }

        public string ConvertMethodName(string methodName)
        {
            return ApplyCamelCaseFormat(methodName);
        }

        public string ConvertArgumentName(string argumentName)
        {
            return ApplyLowercaseAndUnderscoreFormat(argumentName);
        }

        private static string ApplyLowercaseAndUnderscoreFormat(string value)
        {
            var result = new StringBuilder(value.Length * 2);

            foreach (var character in value)
            {
                if (character == Char.ToUpperInvariant(character))
                {
                    result.Append('_');
                    result.Append(char.ToLowerInvariant(character));
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        private static string ApplyCamelCaseFormat(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
