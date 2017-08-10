using System;
using System.IO;

namespace Gameloop.Vdf
{
    public class VdfTextWriter : VdfWriter
    {
        private readonly TextWriter _writer;
        private int _indentationLevel;

        public VdfTextWriter(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
            _indentationLevel = 0;
        }

        public override void WriteKey(string key)
        {
            AutoComplete(State.Key);
            _writer.Write(VdfStructure.Quote);
            _writer.Write(key);
            _writer.Write(VdfStructure.Quote);
        }

        public override void WriteValue(VValue value)
        {
            AutoComplete(State.Value);
            _writer.Write(VdfStructure.Quote);
            _writer.Write(value);
            _writer.Write(VdfStructure.Quote);
        }

        public override void WriteObjectStart()
        {
            AutoComplete(State.ObjectStart);
            _writer.Write(VdfStructure.ObjectStart);

            _indentationLevel++;
        }

        public override void WriteObjectEnd()
        {
            _indentationLevel--;

            AutoComplete(State.ObjectEnd);
            _writer.Write(VdfStructure.ObjectEnd);

            if (_indentationLevel == 0)
                AutoComplete(State.Finished);
        }
        
        private void AutoComplete(State next)
        {
            if (CurrentState == State.Start)
            {
                CurrentState = next;
                return;
            }

            switch (next)
            {
                case State.Value:
                    _writer.Write(VdfStructure.Assign);
                    break;

                case State.Key:
                case State.ObjectStart:
                case State.ObjectEnd:
                    _writer.WriteLine();
                    _writer.Write(new string(VdfStructure.Indent, _indentationLevel));
                    break;

                case State.Finished:
                    _writer.WriteLine();
                    break;
            }

            CurrentState = next;
        }

        public override void Close()
        {
            base.Close();
            if (CloseOutput)
                _writer.Dispose();
        }
    }
}
