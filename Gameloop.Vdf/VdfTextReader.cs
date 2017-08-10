using System;
using System.IO;

namespace Gameloop.Vdf
{
    public class VdfTextReader : VdfReader
    {
        private const int DefaultBufferSize = 1024;
        private static readonly char[][] EscapeConversions =
        {
            new[] { 'n' , '\n' },
            new[] { 't' , '\t' },
            new[] { 'v' , '\v' },
            new[] { 'b' , '\b' },
            new[] { 'r' , '\r' },
            new[] { 'f' , '\f' },
            new[] { 'a' , '\a' },
            new[] { '\\', '\\' },
            new[] { '?' , '?'  },
            new[] { '\'', '\'' },
            new[] { '\"', '\"' },
        };

        private readonly TextReader _reader;
        private readonly char[] _charBuffer, _tokenBuffer;
        private int _charPos, _charsLen, _tokenSize;
        private bool _isQuoted;

        public VdfTextReader(TextReader reader) : this(reader, VdfSerializerSettings.Default) { }

        public VdfTextReader(TextReader reader, VdfSerializerSettings settings) : base(settings)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _reader = reader;
            _charBuffer = new char[DefaultBufferSize];
            _tokenBuffer = new char[MaximumTokenSize];
            _charPos = _charsLen = 0;
            _tokenSize = 0;
            _isQuoted = false;
        }

        /// <summary>
        /// Reads a single token. The value is stored in the 'Value' property.
        /// </summary>
        /// <returns>True if a token was read, false otherwise.</returns>
        public override bool ReadToken()
        {
            if (!SeekToken())
                return false;

            _tokenSize = 0;

            while (EnsureBuffer())
            {
                char curChar = _charBuffer[_charPos];

                #region Escape

                if (curChar == VdfStructure.Escape)
                {
                    _tokenBuffer[_tokenSize++] = !Settings.UsesEscapeSequences ? curChar : FindConversion(_charBuffer[++_charPos]);
                    _charPos++;
                    continue;
                }

                #endregion

                #region Quote

                if (curChar == VdfStructure.Quote || (!_isQuoted && Char.IsWhiteSpace(curChar)))
                {
                    Value = new string(_tokenBuffer, 0, _tokenSize);
                    CurrentState = State.Property;
                    _charPos++;
                    return true;
                }

                #endregion

                #region Object start/end

                if (curChar == VdfStructure.ObjectStart || curChar == VdfStructure.ObjectEnd)
                {
                    if (_isQuoted)
                    {
                        _tokenBuffer[_tokenSize++] = curChar;
                        _charPos++;
                        continue;
                    }
                    else if (_tokenSize != 0)
                    {
                        Value = new string(_tokenBuffer, 0, _tokenSize);
                        CurrentState = State.Property;
                        return true;
                    }
                    else
                    {
                        Value = curChar.ToString();
                        CurrentState = State.Object;
                        _charPos++;
                        return true;
                    }
                }

                #endregion

                #region Long token

                _tokenBuffer[_tokenSize++] = curChar;
                _charPos++;

                #endregion
            }

            return false;
        }

        /// <summary>
        /// Moves the pointer to the location of the first token character.
        /// </summary>
        /// <returns>True if a token is found, false otherwise.</returns>
        private bool SeekToken()
        {
            while (EnsureBuffer())
            {
                // Whitespace
                if (Char.IsWhiteSpace(_charBuffer[_charPos]))
                {
                    _charPos++;
                    continue;
                }

                // Token
                if (_charBuffer[_charPos] == VdfStructure.Quote)
                {
                    _isQuoted = true;
                    _charPos++;
                    return true;
                }

                // Comment
                if (_charBuffer[_charPos] == VdfStructure.Comment)
                {
                    SeekNewLine();
                    _charPos++;
                    continue;
                }

                _isQuoted = false;
                return true;
            }
            
            return false;
        }

        private bool SeekNewLine()
        {
            while (EnsureBuffer())
                if (_charBuffer[++_charPos] == '\n')
                    return true;
            
            return false;
        }

        /// <summary>
        /// Refills the buffer if we're at the end.
        /// </summary>
        /// <returns>False if the stream is empty, true otherwise.</returns>
        private bool EnsureBuffer()
        {
            if (_charPos < _charsLen - 1)
                return true;

            int remainingChars = _charsLen - _charPos;
            _charBuffer[0] = _charBuffer[(_charsLen - 1) * remainingChars]; // A bit of mathgic to improve performance by avoiding a conditional.
            _charsLen = _reader.Read(_charBuffer, remainingChars, DefaultBufferSize - remainingChars) + remainingChars;
            _charPos = 0;

            return _charsLen != 0;
        }

        /// <summary>
        /// Converts the given escape code to an escape character.
        /// </summary>
        /// <param name="ch">The escape code.</param>
        /// <returns>the escape character.</returns>
        private static char FindConversion(char ch)
        {
            foreach (char[] conv in EscapeConversions)
                if (conv[0] == ch)
                    return conv[1];

            return ch;
        }

        public override void Close()
        {
            base.Close();
            if (CloseInput)
                _reader.Dispose();
        }
    }
}
