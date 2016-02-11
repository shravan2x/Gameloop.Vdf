using System;

namespace Gameloop.Vdf
{
    public class VdfSerializer
    {
        public static VdfSerializer Create()
        {
            return new VdfSerializer(VdfSerializerSettings.Default);
        }

        public static VdfSerializer Create(VdfSerializerSettings settings)
        {
            return new VdfSerializer(settings);
        }

        private VdfSerializerSettings _settings;

        public VdfSerializer(VdfSerializerSettings settings)
        {
            _settings = settings;
        }

        public object Deserialize(VdfReader reader)
        {
            reader.ReadToken();
            return ReadProperty(reader);
        }

        private VProperty ReadProperty(VdfReader reader)
        {
            VProperty result = new VProperty();
            result.Key = reader.Value;

            reader.ReadToken();
            if (reader.CurrentState == VdfReaderState.Property)
                result.Value = new VValue(reader.Value);
            else
                result.Value = ReadObject(reader);

            return result;
        }

        private VObject ReadObject(VdfReader reader)
        {
            VObject result = new VObject();

            reader.ReadToken();
            do
            {
                result.Add(ReadProperty(reader));
                reader.ReadToken();
            }
            while (reader.CurrentState != VdfReaderState.Object || reader.Value != VdfStructure.ObjectEnd.ToString());

            return result;
        }
    }
}
