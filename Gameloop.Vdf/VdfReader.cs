using System;

namespace Gameloop.Vdf
{
    public abstract class VdfReader : IDisposable
    {
        protected const int MaximumTokenSize = 4096;

        public VdfSerializerSettings Settings { get; }
        public VdfReaderState CurrentState;
        public bool CloseInput { get; set; }
        public string Value { get; set; }

        protected VdfReader()
        {
            Settings = VdfSerializerSettings.Default;
            CurrentState = VdfReaderState.Start;
            Value = null;
            CloseInput = true;
        }

        protected VdfReader(VdfSerializerSettings settings)
        {
            Settings = settings;

            CurrentState = VdfReaderState.Start;
            Value = null;
            CloseInput = true;
        }

        public abstract bool ReadToken();

        void IDisposable.Dispose()
        {
            if (CurrentState == VdfReaderState.Closed)
                return;

            Close();
        }

        public virtual void Close()
        {
            CurrentState = VdfReaderState.Closed;
            Value = null;
        }
    }

    public enum VdfReaderState
    {
        Start,
        Property,
        Object,
        Conditional,
        Finished,
        Closed
    }
}
