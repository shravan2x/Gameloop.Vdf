using System;

namespace Gameloop.Vdf.Linq
{
    public class VProperty : VToken
    {
        public string Key { get; set; }
        public VToken Value { get; set; }

        public VProperty() { }

        public VProperty(string key, VToken value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Key = key;
            Value = value;
        }

        public override VTokenType Type => VTokenType.Property;

        public override void WriteTo(VdfWriter writer)
        {
            writer.WriteKey(Key);
            Value.WriteTo(writer);
        }
    }
}
