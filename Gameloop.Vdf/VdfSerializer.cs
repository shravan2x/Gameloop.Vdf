using Gameloop.Vdf.Linq;
using System.IO;

namespace Gameloop.Vdf
{
    public class VdfSerializer
    {
        private readonly VdfSerializerSettings _settings;

        public VdfSerializer() : this(VdfSerializerSettings.Default) { }

        public VdfSerializer(VdfSerializerSettings settings)
        {
            _settings = settings;
        }

        public void Serialize(TextWriter textWriter, VToken value)
        {
            using VdfWriter vdfWriter = new VdfTextWriter(textWriter, _settings);
            value.WriteTo(vdfWriter);
        }

        public VProperty Deserialize(TextReader textReader)
        {
            using VdfReader vdfReader = new VdfTextReader(textReader, _settings);

            if (!vdfReader.ReadToken())
                throw new VdfException("Incomplete VDF data.");

            // For now, we discard these comments.
            while (vdfReader.CurrentState == VdfReader.State.Comment)
                if (!vdfReader.ReadToken())
                    throw new VdfException("Incomplete VDF data.");

            return ReadProperty(vdfReader);
        }

        private VProperty ReadProperty(VdfReader reader)
        {
            // Setting it to null is temporary, we'll set Value in just a second.
            VProperty result = new VProperty(reader.Value, null!);

            if (!reader.ReadToken())
                throw new VdfException("Incomplete VDF data.");

            // For now, we discard these comments.
            while (reader.CurrentState == VdfReader.State.Comment)
                if (!reader.ReadToken())
                    throw new VdfException("Incomplete VDF data.");

            if (reader.CurrentState == VdfReader.State.Property)
                result.Value = new VValue(reader.Value);
            else
                result.Value = ReadObject(reader);

            return result;
        }

        private VObject ReadObject(VdfReader reader)
        {
            VObject result = new VObject();

            if (!reader.ReadToken())
                throw new VdfException("Incomplete VDF data.");

            while (!(reader.CurrentState == VdfReader.State.Object && reader.Value == VdfStructure.ObjectEnd.ToString()))
            {
                if (reader.CurrentState == VdfReader.State.Comment)
                    result.Add(VValue.CreateComment(reader.Value));
                else
                    result.Add(ReadProperty(reader));

                if (!reader.ReadToken())
                    throw new VdfException("Incomplete VDF data.");
            }

            return result;
        }
    }
}
