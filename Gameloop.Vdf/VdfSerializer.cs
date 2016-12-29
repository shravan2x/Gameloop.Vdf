using System.IO;

namespace Gameloop.Vdf
{
    public class VdfSerializer
    {
        private readonly VdfSerializerSettings _settings;

        public VdfSerializer(VdfSerializerSettings settings)
        {
            _settings = settings;
        }

        public VProperty Deserialize(TextReader textReader)
        {
            using (VdfReader vdfReader = new VdfTextReader(textReader, _settings))
            {
                vdfReader.ReadToken();
                return ReadProperty(vdfReader);
            }
        }

        private VProperty ReadProperty(VdfReader reader)
        {
            VProperty result = new VProperty();
            result.Key = reader.Value;

            reader.ReadToken();
            if (reader.CurrentState == VdfReader.State.Property)
                result.Value = new VValue(reader.Value);
            else
                result.Value = ReadObject(reader);

            return result;
        }

        private VObject ReadObject(VdfReader reader)
        {
            VObject result = new VObject();

            reader.ReadToken();
            while (reader.CurrentState != VdfReader.State.Object || reader.Value != VdfStructure.ObjectEnd.ToString())
            {
                result.Add(ReadProperty(reader));
                reader.ReadToken();
            }

            return result;
        }
    }
}
