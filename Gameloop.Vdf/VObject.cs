using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Gameloop.Vdf
{
    public class VObject : VToken, ITypedList, ICustomTypeDescriptor
    {
        private readonly List<VProperty> _children;

        public int Count => _children.Count;

        public VToken this[string key]
        {
            get
            {
                VProperty result;
                if (!TryGetValue(key, out result))
                    throw new KeyNotFoundException("The given key was not present.");

                return result.Value;
            }

            set
            {
                VProperty result;
                if (TryGetValue(key, out result))
                {
                    result.Value = value;
                    return;
                }

                Add(key, new VProperty(key, value));
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

        #region ICustomTypeDescriptor Methods

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection descriptorCollection = new PropertyDescriptorCollection(null);

            foreach (VProperty property in _children)
                descriptorCollection.Add(new VPropertyDescriptor(property.Key));

            return descriptorCollection;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public AttributeCollection GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        public string GetClassName()
        {
            return null;
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return new TypeConverter();
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return null;
        }

        #endregion

        #region ITypedList Methods

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return null;
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            Console.WriteLine("hi");
            return GetProperties();
        }

        #endregion
    }
}
