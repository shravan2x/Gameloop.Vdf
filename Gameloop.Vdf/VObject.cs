using Gameloop.Vdf.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Gameloop.Vdf
{
    public class VObject : VToken
    {
        private readonly List<VProperty> _children;

        public int Count => _children.Count;

        public VToken this[string key]
        {
            get
            {
                if (!TryGetValue(key, out VProperty result))
                    throw new KeyNotFoundException("The given key was not present.");

                return result.Value;
            }

            set
            {
                if (TryGetValue(key, out VProperty result))
                    result.Value = value;
                else
                    Add(key, value);
            }
        }

        public VObject()
        {
            _children = new List<VProperty>();
        }

        public IEnumerable<VProperty> Children()
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

        public bool TryGetValue(string key, out VProperty value)
        {
            value = _children.FirstOrDefault(x => x.Key == key);
            return value != null;
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
