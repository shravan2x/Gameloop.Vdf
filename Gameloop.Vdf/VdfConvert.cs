using Gameloop.Vdf.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Gameloop.Vdf
{
    public static class VdfConvert
    {
        public static string Serialize(VToken value)
        {
            return Serialize(value, VdfSerializerSettings.Common);
        }

        public static string Serialize(VToken value, VdfSerializerSettings settings)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            StringBuilder stringBuilder = new StringBuilder(256);
            StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
            (new VdfSerializer(settings)).Serialize(stringWriter, value);

            return stringWriter.ToString();
        }

        public static VProperty Deserialize(string value)
        {
            return Deserialize(value, VdfSerializerSettings.Common);
        }

        public static VProperty Deserialize(string value, VdfSerializerSettings settings)
        {
            return Deserialize(new StringReader(value), settings);
        }

        public static VProperty Deserialize(TextReader reader)
        {
            return Deserialize(reader, VdfSerializerSettings.Common);
        }

        public static VProperty Deserialize(TextReader reader, VdfSerializerSettings settings)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            
            return (new VdfSerializer(settings)).Deserialize(reader);
        }
    }
}
