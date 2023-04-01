using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameloop.Vdf.Linq
{
    public class VConditional : VToken
    {
        public const string X360 = "X360";
        public const string Ps3 = "PS3";
        public const string Win32 = "WIN32";
        public const string OsX = "OSX";
        public const string Windows = "WINDOWS";
        public const string Linux = "LINUX";
        public const string Posix = "POSIX";

        private readonly List<Token> _tokens;

        public VConditional()
        {
            _tokens = new List<Token>();
        }

        public override VTokenType Type => VTokenType.Conditional;
        public IReadOnlyList<Token> Tokens => _tokens;

        public override VToken DeepClone()
        {
            VConditional newCond = new VConditional();
            foreach (Token token in _tokens)
                newCond.Add(token.DeepClone());

            return newCond;
        }

        public override void WriteTo(VdfWriter writer)
        {
            writer.WriteConditional(_tokens);
        }

        protected override bool DeepEquals(VToken token)
        {
            if (token is not VConditional otherCond)
                return false;

            return (_tokens.Count == otherCond._tokens.Count && Enumerable.Range(0, _tokens.Count).All(x => Token.DeepEquals(_tokens[x], otherCond._tokens[x])));
        }

        public void Add(Token token)
        {
            _tokens.Add(token);
        }

        public bool Evaluate(IReadOnlyList<string> definedConditionals)
        {
            int index = 0;

            bool EvaluateToken()
            {
                if (_tokens[index].TokenType != TokenType.Not && _tokens[index].TokenType != TokenType.Constant)
                    throw new Exception($"Unexpected conditional token type ({_tokens[index].TokenType}).");

                bool isNot = false;
                if (_tokens[index].TokenType == TokenType.Not)
                {
                    isNot = true;
                    index++;
                }

                if (_tokens[index].TokenType != TokenType.Constant)
                    throw new Exception($"Unexpected conditional token type ({_tokens[index].TokenType}).");

                return (isNot ^ definedConditionals.Contains(_tokens[index++].Name!));
            }

            bool runningResult = EvaluateToken();
            while (index < _tokens.Count)
            {
                TokenType tokenType = _tokens[index++].TokenType;

                if (tokenType == TokenType.Or)
                    runningResult |= EvaluateToken();
                else if (tokenType == TokenType.And)
                    runningResult &= EvaluateToken();
                else
                    throw new Exception($"Unexpected conditional token type ({tokenType}).");
            }

            return runningResult;
        }

        public readonly struct Token
        {
            public TokenType TokenType { get; }
            public string? Name { get; }

            public Token(TokenType tokenType, string? name = null)
            {
                TokenType = tokenType;
                Name = name;
            }

            public Token DeepClone()
            {
                return new Token(TokenType, Name);
            }

            public static bool DeepEquals(Token t1, Token t2)
            {
                return (t1.TokenType == t2.TokenType && t1.Name == t2.Name);
            }
        }

        public enum TokenType
        {
            Constant,
            Not,
            Or,
            And
        }
    }
}
