using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.CodeConventions
{
    [DebuggerTypeProxy(typeof(DebuggerProxyView))]
    public class ObjectAccessDecorator : DynamicObject
    {
        private readonly ICodeConvention _convention;
        private readonly IDictionary<string, object> _source;

        public ObjectAccessDecorator(ICodeConvention convention, ExpandoObject source)
        {
            this._convention = convention;
            this._source = source as IDictionary<string, object>;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var hasIndex = base.TryGetIndex(binder, indexes, out result);

            if (!hasIndex)
            {
                return false;
            }

            if(indexes.Length == 1)
            {
                result = this.Decorate(result);
                this._source[(string)indexes[0]] = result;
            }

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = this._convention.ConvertPropertyName(binder.Name);

            if (!this._source.ContainsKey(propertyName))
            {
                result = null;
                return false;
            }

            result = this._source[propertyName];

            if (result == null)
            {
                return true;
            }

            var updateProperty = result.GetType() == typeof(ExpandoObject);
            result = this.Decorate(result);

            if (updateProperty)
            {
                this._source[propertyName] = result;
            }

            return true;
        }

        public object Decorate(object source)
        {
            if (source == null)
            {
                return source;
            }

            var sourceType = source.GetType();

            if (sourceType.IsArray)
            {
                var resultArray = source as object[];
                var newArrayResult = new dynamic[resultArray.Length];

                for (var i = 0; i < resultArray.Length; ++i)
                {
                    var item = resultArray[i];

                    newArrayResult[i] = item;

                    if (item == null)
                    {
                        continue;
                    }

                    var itemType = item.GetType();

                    if (itemType.IsValueType)
                    {
                        continue;
                    }

                    if (itemType == typeof(ExpandoObject))
                    {
                        newArrayResult[i] = this.Decorate(item);
                    }
                }

                source = newArrayResult;
            }

            if (source is ObjectAccessDecorator)
            {
                return source;
            }

            if (sourceType == typeof(ExpandoObject))
            {
                source = new ObjectAccessDecorator(this._convention, (ExpandoObject)source);
            }

            return source;
        }

        internal class DebuggerProxyView : IDictionary<string, object>
        {
            private readonly ICodeConvention _convention;
            private readonly IDictionary<string, object> _source;

            public DebuggerProxyView(ObjectAccessDecorator decorator)
            {
                this._source = decorator._source;
                this._convention = decorator._convention;
            }

            public bool ContainsKey(string key)
            {
                return this._source.ContainsKey(this._convention.ConvertPropertyName(key));
            }

            public ICollection<string> Keys
            {
                get { return this._source.Keys.Select(x => this._convention.ConvertPropertyName(x)).ToArray(); }
            }

            public bool TryGetValue(string key, out object value)
            {
                return this._source.TryGetValue(key, out value);
            }

            public ICollection<object> Values
            {
                get { return this._source.Values; }
            }

            public object this[string key]
            {
                get
                {
                    return this._source[this._convention.ConvertPropertyName(key)];
                }
                set
                {
                    throw new NotImplementedException();
                }
            }
            public bool Contains(KeyValuePair<string, object> item)
            {
                var propertyName = this._convention.ConvertPropertyName(item.Key);
                return this._source.Contains(new KeyValuePair<string, object>(propertyName, item.Value));
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                this._source.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return this._source.Count(); }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                return this._source.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this._source.GetEnumerator();
            }

            // Write operations

            public void Add(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void Add(string key, object value)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }
        }
    }
}
