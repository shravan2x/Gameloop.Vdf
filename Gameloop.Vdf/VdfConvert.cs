using System;
using System.IO;

namespace Gameloop.Vdf
{
    public static class VdfConvert
    {
        public static object DeserializeObject(string value)
        {
            return DeserializeObject(new StringReader(value), VdfSerializerSettings.Default);
        }

        public static object DeserializeObject(TextReader reader)
        {
            return DeserializeObject(reader, VdfSerializerSettings.Default);
        }

        public static object DeserializeObject(TextReader reader, VdfSerializerSettings settings)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            VdfSerializer serializer = VdfSerializer.Create(settings);

            using (VdfTextReader jsonTextReader = new VdfTextReader(reader))
                return serializer.Deserialize(jsonTextReader);
        }
    }
}
