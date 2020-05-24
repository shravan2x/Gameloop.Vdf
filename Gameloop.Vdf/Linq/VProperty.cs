using System;

namespace Gameloop.Vdf.Linq
{
    public class VProperty : VToken
    {
        // Json.NET calls this 'Name', but since VDF is technically KeyValues we call it a 'Key'.
        public string Key { get; set; }
        public VToken Value { get; set; }

        public VProperty(string key, VToken value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Key = key;
            Value = value;
        }

        public VProperty(VProperty other)
            : this(other.Key, other.Value.DeepClone()) { }

        public override VTokenType Type => VTokenType.Property;

        public override VToken DeepClone()
        {
            return new VProperty(this);
        }

        public override void WriteTo(VdfWriter writer)
        {
            writer.WriteKey(Key);
            Value.WriteTo(writer);
        }

        protected override bool DeepEquals(VToken node)
        {
            return (node is VProperty otherProp && Key == otherProp.Key && VToken.DeepEquals(Value, otherProp.Value));
        }
    }
}
