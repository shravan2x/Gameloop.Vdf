using System;

namespace Gameloop.Vdf.Linq
{
    public class VValue : VToken
    {
        public object Value { get; set; }

        public VValue(object value)
        {
            Value = value;
        }

        public override void WriteTo(VdfWriter writer)
        {
            writer.WriteValue(this);
        }

        public override string ToString()
        {
            return Value?.ToString() ?? String.Empty;
        }
    }
}
