using System;
using System.ComponentModel;

namespace Gameloop.Vdf
{
    public class VPropertyDescriptor : PropertyDescriptor
    {
        public VPropertyDescriptor(string name) : base(name, null) { }

        public override Type ComponentType => typeof(VObject);
        public override bool IsReadOnly => false;
        public override Type PropertyType => typeof(object);

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            Console.WriteLine("hi");
            return ((VObject) component)[Name];
        }

        public override void ResetValue(object component) { }

        public override void SetValue(object component, object value)
        {
            ((VObject) component)[Name] = (value is VToken) ? (VToken) value : new VValue(value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
