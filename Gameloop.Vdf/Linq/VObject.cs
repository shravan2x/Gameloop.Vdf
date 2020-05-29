using Gameloop.Vdf.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Gameloop.Vdf.Linq
{
    public class VObject : VToken, IList<VToken>, IDictionary<string, VToken>
    {
        private readonly List<VToken> _children;

        public VObject()
        {
            _children = new List<VToken>();
        }

        public VObject(VObject other)
        {
            _children = other._children.Select(x => x.DeepClone()).ToList();
        }

        public override VTokenType Type => VTokenType.Object;

        public int Count => _children.Count;

        public override VToken? this[object key]
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

        public VToken this[int index]
        {
            get => _children[index];
            set => _children[index] = value;
        }

        public VToken? this[string key]
        {
            get
            {
                if (!TryGetValue(key, out VToken? result))
                    return null;

                return result;
            }

            set
            {
                VProperty prop = Properties().FirstOrDefault(x => x.Key == key);
                if (prop != null)
                    prop.Value = value ?? VValue.CreateEmpty();
                else
                    Add(key, value ?? VValue.CreateEmpty());
            }
        }

        public bool IsReadOnly => false;

        ICollection<string> IDictionary<string, VToken>.Keys => Properties().Select(x => x.Key).ToList();

        ICollection<VToken> IDictionary<string, VToken>.Values => throw new NotImplementedException();

        public override IEnumerable<VToken> Children()
        {
            return _children;
        }

        public IEnumerable<VProperty> Properties()
        {
            return _children.Where(x => x is VProperty).OfType<VProperty>();
        }

        public void Add(string key, VToken value)
        {
            Add(new VProperty(key, value));
        }

        public void Add(VProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (property.Value == null)
                throw new ArgumentNullException(nameof(property.Value));

            _children.Add(property);
        }

        public void Add(VToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            _children.Add(token);
        }

        public void Clear()
        {
            _children.Clear();
        }

        public bool Contains(VToken item)
        {
            return _children.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return Properties().Any(x => x.Key == key);
        }

        public void CopyTo(VToken[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public override VToken DeepClone()
        {
            return new VObject(this);
        }

        public int IndexOf(VToken item)
        {
            return _children.IndexOf(item);
        }

        public void Insert(int index, VToken item)
        {
            _children.Insert(index, item);
        }

        public bool Remove(string key)
        {
            return _children.RemoveAll(x => x is VProperty p && p.Key == key) != 0;
        }

        public bool Remove(VToken item)
        {
            return _children.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _children.RemoveAt(index);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out VToken value)
        {
            value = Properties().FirstOrDefault(x => x.Key == key)?.Value;
            return (value != null);
        }

        public override void WriteTo(VdfWriter writer)
        {
            writer.WriteObjectStart();

            foreach (VToken child in _children)
                child.WriteTo(writer);

            writer.WriteObjectEnd();
        }

        #region ICollection<KeyValuePair<string,JToken>> Members

        public IEnumerator<KeyValuePair<string, VToken>> GetEnumerator()
        {
            foreach (VProperty property in Properties())
                yield return new KeyValuePair<string, VToken>(property.Key, property.Value);
        }

        VToken IDictionary<string, VToken>.this[string key]
        {
            get => this[key] ?? throw new KeyNotFoundException();
            set => this[key] = value ?? throw new ArgumentNullException(nameof(value));
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
            VProperty property = Properties().FirstOrDefault(x => x.Key == item.Key);
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

            int index = 0;
            foreach (VProperty property in Properties())
                array[arrayIndex + index++] = new KeyValuePair<string, VToken>(property.Key, property.Value);
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

        protected override bool DeepEquals(VToken token)
        {
            if (!(token is VObject otherObj))
                return false;

            return (_children.Count == otherObj._children.Count && Enumerable.Range(0, _children.Count).All(x => VToken.DeepEquals(_children[x], otherObj._children[x])));
        }

        protected override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicProxyMetaObject<VObject>(parameter, this, new VObjectDynamicProxy());
        }

        private class VObjectDynamicProxy : DynamicProxy<VObject>
        {
            public override bool TryGetMember(VObject instance, GetMemberBinder binder, out object? result)
            {
                // result can be null
                result = instance[binder.Name];
                return true;
            }

            public override bool TrySetMember(VObject instance, SetMemberBinder binder, object value)
            {
                // this can throw an error if value isn't a valid for a JValue
                if (!(value is VToken v))
                    v = new VValue(value);

                instance[binder.Name] = v;
                return true;
            }

            public override IEnumerable<string> GetDynamicMemberNames(VObject instance)
            {
                return instance.Properties().Select(p => p.Key);
            }
        }
    }
}
