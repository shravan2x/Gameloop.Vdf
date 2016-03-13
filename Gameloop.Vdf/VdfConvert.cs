using System;
using System.IO;

namespace Gameloop.Vdf
{
    public static class VdfConvert
    {
        public static VProperty Deserialize(string value)
        {
            return Deserialize(value, VdfSerializerSettings.Default);
        }

        public static VProperty Deserialize(string value, VdfSerializerSettings settings)
        {
            return Deserialize(new StringReader(value), settings);
        }

        public static VProperty Deserialize(TextReader reader)
        {
            return Deserialize(reader, VdfSerializerSettings.Default);
        }

        public static VProperty Deserialize(TextReader reader, VdfSerializerSettings settings)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            
            return (new VdfSerializer(settings)).Deserialize(reader);
        }
    }
}
