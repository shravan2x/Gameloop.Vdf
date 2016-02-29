using System;

namespace Gameloop.Vdf
{
    public abstract class VdfReader : IDisposable
    {
        protected const int MaximumTokenSize = 4096;

        public VdfSerializerSettings Settings { get; }
        public EVdfReaderState CurrentState;
        public bool CloseInput { get; set; }
        public string Value { get; set; }

        protected VdfReader()
        {
            Settings = VdfSerializerSettings.Default;
            CurrentState = EVdfReaderState.Start;
            Value = null;
            CloseInput = true;
        }

        protected VdfReader(VdfSerializerSettings settings)
        {
            Settings = settings;

            CurrentState = EVdfReaderState.Start;
            Value = null;
            CloseInput = true;
        }

        public abstract bool ReadToken();

        void IDisposable.Dispose()
        {
            if (CurrentState == EVdfReaderState.Closed)
                return;

            Close();
        }

        public virtual void Close()
        {
            CurrentState = EVdfReaderState.Closed;
            Value = null;
        }
    }

    public enum EVdfReaderState
    {
        Start,
        Property,
        Object,
        Conditional,
        Finished,
        Closed
    }
}
