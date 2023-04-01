using Gameloop.Vdf.Linq;
using System;
using System.IO;

namespace Gameloop.Vdf
{
    public class VdfSerializer
    {
        private readonly VdfSerializerSettings _settings;

        public VdfSerializer()
            : this(VdfSerializerSettings.Default) { }

        public VdfSerializer(VdfSerializerSettings settings)
        {
            _settings = settings;

            if (_settings.UsesConditionals && _settings.DefinedConditionals == null)
                throw new Exception("DefinedConditionals must be set when UsesConditionals=true.");
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
                throw new VdfException("Incomplete VDF data at beginning of file.");

            // For now, we discard these comments.
            while (vdfReader.CurrentState == VdfReader.State.Comment)
                if (!vdfReader.ReadToken())
                    throw new VdfException("Incomplete VDF data after root comment.");

            return ReadProperty(vdfReader);
        }

        private VProperty ReadProperty(VdfReader reader)
        {
            // Setting it to null is temporary, we'll set Value in just a second.
            VProperty result = new VProperty(reader.Value, null!);

            if (!reader.ReadToken())
                throw new VdfException("Incomplete VDF data after property key.");

            // For now, we discard these comments.
            while (reader.CurrentState == VdfReader.State.Comment)
                if (!reader.ReadToken())
                    throw new VdfException("Incomplete VDF data after property comment.");

            if (reader.CurrentState == VdfReader.State.Property)
            {
                result.Value = new VValue(reader.Value);

                if (!reader.ReadToken())
                    throw new VdfException("Incomplete VDF data after property value.");

                if (reader.CurrentState == VdfReader.State.Conditional)
                    result.Conditional = ReadConditional(reader);
            }
            else if (reader.CurrentState == VdfReader.State.Object)
                result.Value = ReadObject(reader);
            else
                throw new VdfException($"Unexpected state when deserializing property (key: {result.Key}, state: {reader.CurrentState}).");

            return result;
        }

        private VObject ReadObject(VdfReader reader)
        {
            VObject result = new VObject();

            if (!reader.ReadToken())
                throw new VdfException("Incomplete VDF data after object start.");

            while (!(reader.CurrentState == VdfReader.State.Object && reader.Value == VdfStructure.ObjectEnd.ToString()))
            {
                if (reader.CurrentState == VdfReader.State.Comment)
                {
                    result.Add(VValue.CreateComment(reader.Value));

                    if (!reader.ReadToken())
                        throw new VdfException("Incomplete VDF data after object comment.");
                }
                else if (reader.CurrentState == VdfReader.State.Property)
                {
                    VProperty prop = ReadProperty(reader);

                    if (!_settings.UsesConditionals || prop.Conditional == null || prop.Conditional.Evaluate(_settings.DefinedConditionals!))
                        result.Add(prop);
                }
                else
                    throw new VdfException($"Unexpected state when deserializing (state: {reader.CurrentState}, value: {reader.Value}).");
            }

            reader.ReadToken();

            return result;
        }

        private VConditional ReadConditional(VdfReader reader)
        {
            VConditional result = new VConditional();

            if (!reader.ReadToken())
                throw new VdfException("Incomplete VDF data after conditional start.");

            while (reader.CurrentState == VdfReader.State.Conditional && reader.Value != VdfStructure.ConditionalEnd.ToString())
            {
                if (reader.Value == "!")
                    result.Add(new VConditional.Token(VConditional.TokenType.Not, null));
                else if (reader.Value == "&&")
                    result.Add(new VConditional.Token(VConditional.TokenType.And, null));
                else if (reader.Value == "||")
                    result.Add(new VConditional.Token(VConditional.TokenType.Or, null));
                else
                    result.Add(new VConditional.Token(VConditional.TokenType.Constant, reader.Value.Substring(1)));

                if (!reader.ReadToken())
                    throw new VdfException("Incomplete VDF data after conditional expression.");
            }

            if (!reader.ReadToken())
                throw new VdfException("Incomplete VDF data after conditional end.");

            return result;
        }
    }
}
