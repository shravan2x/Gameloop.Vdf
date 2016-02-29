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
            if (reader.CurrentState == EVdfReaderState.Property)
                result.Value = new VValue(reader.Value);
            else
                result.Value = ReadObject(reader);

            return result;
        }

        private VObject ReadObject(VdfReader reader)
        {
            VObject result = new VObject();

            reader.ReadToken();
            while (reader.CurrentState != EVdfReaderState.Object || reader.Value != VdfStructure.ObjectEnd.ToString())
            {
                result.Add(ReadProperty(reader));
                reader.ReadToken();
            }

            return result;
        }
    }
}
