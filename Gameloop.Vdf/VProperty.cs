using System;

namespace Gameloop.Vdf
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
    }
}
