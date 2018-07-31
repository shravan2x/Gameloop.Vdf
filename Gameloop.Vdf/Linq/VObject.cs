using Gameloop.Vdf.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Gameloop.Vdf.Linq
{
    public class VObject : VToken, IDictionary<string, VToken>
    {
        private readonly List<VProperty> _children;

        public VObject()
        {
            _children = new List<VProperty>();
        }

        public int Count => _children.Count;

        public override VToken this[object key]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(key, nameof(key));

                if (!(key is string propertyName))
                    throw new ArgumentException($"Accessed JObject values with invalid key value: {MiscellaneousUtils.ToString(key)}. Object property name expected.");

                return this[propertyName];
            }
            set
            {
                ValidationUtils.ArgumentNotNull(key, nameof(key));

                if (!(key is string propertyName))
                    throw new ArgumentException($"Set JObject values with invalid key value: {MiscellaneousUtils.ToString(key)}. Object property name expected.");

                this[propertyName] = value;
            }
        }

        public VToken this[string key]
        {
            get
            {
                if (!TryGetValue(key, out VToken result))
                    return null;

                return result;
            }

            set
            {
                VProperty prop = _children.FirstOrDefault(x => x.Key == key);
                if (prop != null)
                    prop.Value = value;
                else
                    Add(key, value);
            }
        }

        ICollection<string> IDictionary<string, VToken>.Keys => _children.Select(x => x.Key).ToList();

        ICollection<VToken> IDictionary<string, VToken>.Values => throw new NotImplementedException();

        public override IEnumerable<VProperty> Children()
        {
            return _children;
        }

        public void Add(string key, VToken value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _children.Add(new VProperty(key, value));
        }

        public void Add(VProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (property.Value == null)
                throw new ArgumentNullException(nameof(property.Value));

            _children.Add(property);
        }

        public void Clear()
        {
            _children.Clear();
        }

        public bool ContainsKey(string key)
        {
            return _children.Exists(x => x.Key == key);
        }

        public void CopyTo(VProperty[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public bool Remove(string key)
        {
            return _children.RemoveAll(x => x.Key == key) != 0;
        }

        public bool TryGetValue(string key, out VToken value)
        {
            value = _children.FirstOrDefault(x => x.Key == key)?.Value;
            return (value != null);
        }

        public void RemoveAt(string key)
        {
            _children.RemoveAll(x => x.Key == key);
        }

        public override void WriteTo(VdfWriter writer)
        {
            writer.WriteObjectStart();

            foreach (VProperty child in _children)
                child.WriteTo(writer);

            writer.WriteObjectEnd();
        }

        #region ICollection<KeyValuePair<string,JToken>> Members

        public IEnumerator<KeyValuePair<string, VToken>> GetEnumerator()
        {
            foreach (VProperty property in _children)
                yield return new KeyValuePair<string, VToken>(property.Key, property.Value);
        }

        void ICollection<KeyValuePair<string, VToken>>.Add(KeyValuePair<string, VToken> item)
        {
            Add(new VProperty(item.Key, item.Value));
        }

        void ICollection<KeyValuePair<string, VToken>>.Clear()
        {
            _children.Clear();
        }

        bool ICollection<KeyValuePair<string, VToken>>.Contains(KeyValuePair<string, VToken> item)
        {
            VProperty property = _children.FirstOrDefault(x => x.Key == item.Key);
            if (property == null)
                return false;

            return (property.Value == item.Value);
        }

        void ICollection<KeyValuePair<string, VToken>>.CopyTo(KeyValuePair<string, VToken>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than 0.");
            if (arrayIndex >= array.Length && arrayIndex != 0)
                throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
            if (Count > array.Length - arrayIndex)
                throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");

            for (int index = 0; index < _children.Count; index++)
                array[arrayIndex + index] = new KeyValuePair<string, VToken>(_children[index].Key, _children[index].Value);
        }

        bool ICollection<KeyValuePair<string, VToken>>.IsReadOnly => false;

        bool ICollection<KeyValuePair<string, VToken>>.Remove(KeyValuePair<string, VToken> item)
        {
            if (!((ICollection<KeyValuePair<string, VToken>>) this).Contains(item))
                return false;

            ((IDictionary<string, VToken>) this).Remove(item.Key);
            return true;
        }

        #endregion

        protected override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicProxyMetaObject<VObject>(parameter, this, new VObjectDynamicProxy());
        }

        private class VObjectDynamicProxy : DynamicProxy<VObject>
        {
            public override bool TryGetMember(VObject instance, GetMemberBinder binder, out object result)
            {
                // result can be null
                result = instance[binder.Name];
                return true;
            }

            public override bool TrySetMember(VObject instance, SetMemberBinder binder, object value)
            {
                VToken v = value as VToken;

                // this can throw an error if value isn't a valid for a JValue
                if (v == null)
                    v = new VValue(value);

                instance[binder.Name] = v;
                return true;
            }

            public override IEnumerable<string> GetDynamicMemberNames(VObject instance)
            {
                return instance.Children().Select(p => p.Key);
            }
        }
    }
}
