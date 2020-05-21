using Gameloop.Vdf.Linq;
using System;

namespace Gameloop.Vdf
{
    public abstract class VdfWriter : IDisposable
    {
        public VdfSerializerSettings Settings { get; }
        public bool CloseOutput { get; set; }
        protected internal State CurrentState { get; protected set; }

        protected VdfWriter() : this(VdfSerializerSettings.Default) { }

        protected VdfWriter(VdfSerializerSettings settings)
        {
            Settings = settings;

            CurrentState = State.Start;
            CloseOutput = true;
        }

        public abstract void WriteObjectStart();

        public abstract void WriteObjectEnd();

        public abstract void WriteKey(string key);

        public abstract void WriteValue(VValue value);

        public abstract void WriteComment(string text);

        void IDisposable.Dispose()
        {
            if (CurrentState == State.Closed)
                return;

            Close();
        }

        public virtual void Close()
        {
            CurrentState = State.Closed;
        }

        protected internal enum State
        {
            Start,
            Key,
            Value,
            ObjectStart,
            ObjectEnd,
            Comment,
            Finished,
            Closed
        }
    }
}
