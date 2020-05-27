using System;

namespace Gameloop.Vdf.Linq
{
    public class VValue : VToken
    {
        private readonly VTokenType _tokenType;
        public object? Value { get; set; }

        private VValue(object? value, VTokenType type)
        {
            Value = value;
            _tokenType = type;
        }

        public VValue(object? value)
            : this(value, VTokenType.Value) { }

        public VValue(VValue other)
            : this(other.Value, other.Type) { }

        public override VTokenType Type => _tokenType;

        public override VToken DeepClone()
        {
            return new VValue(this);
        }

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

        public static VValue CreateEmpty()
        {
            return new VValue(String.Empty);
        }

        protected override bool DeepEquals(VToken token)
        {
            if (!(token is VValue otherVal))
                return false;

            return (this == otherVal || (Type == otherVal.Type && Value != null && Value.Equals(otherVal.Value)));
        }
    }
}
