using System;
using System.Linq;

namespace UserApp.Core
{
    public static class ReflectionUtility
    {
        public static void CopyProperties(object source, object target, Func<Type, bool> predicate = null)
        {
            var sourceType = source.GetType();
            var targetProperties = target.GetType().GetProperties().ToDictionary(x => x.Name, x => x);

            foreach (var property in sourceType.GetProperties())
            {
                if (!targetProperties.ContainsKey(property.Name))
                {
                    throw new ArgumentException("Unable to set property '" + property.Name + "'. Property does not exist.");
                }

                var selfProperty = targetProperties[property.Name];
                var selfPropertyType = selfProperty.PropertyType;

                if (predicate != null)
                {
                    if (!predicate(selfPropertyType))
                    {
                        continue;
                    }
                }

                if (selfPropertyType != property.PropertyType)
                {
                    throw new ArgumentException(string.Format("Invalid type of property '{0}'. Expected '{1}' got '{2}'.",
                        property.Name, selfPropertyType.Name, property.PropertyType.Name));
                }

                selfProperty.SetValue(target, property.GetValue(source));
            }
        }
    }
}
