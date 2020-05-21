using System;

namespace Gameloop.Vdf.Linq
{
    public class VValue : VToken
    {
        private readonly VTokenType _tokenType;
        public object Value { get; set; }

        private VValue(object value, VTokenType type)
        {
            Value = value;
            _tokenType = type;
        }

        public VValue(object value)
            : this(value, VTokenType.Value) { }

        public override VTokenType Type => _tokenType;

        public override void WriteTo(VdfWriter writer)
        {
            if (_tokenType == VTokenType.Comment)
                writer.WriteComment(ToString());
            else
                writer.WriteValue(this);
        }

        public override string ToString()
        {
            return Value?.ToString() ?? String.Empty;
        }

        public static VValue CreateComment(string value)
        {
            return new VValue(value, VTokenType.Comment);
        }
    }
}
