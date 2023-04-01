using System.Collections.Generic;

namespace Gameloop.Vdf
{
    public class VdfSerializerSettings
    {
        public static VdfSerializerSettings Default => new VdfSerializerSettings();
        public static VdfSerializerSettings Common => new VdfSerializerSettings
        {
            UsesEscapeSequences = true,
            UsesConditionals = false
        };

        /// <summary>
        /// Determines whether the parser should translate escape sequences (/n, /t, etc.).
        /// </summary>
        public bool UsesEscapeSequences = false;

        /// <summary>
        /// Determines whether the parser should evaluate conditional blocks ([$WINDOWS], etc.).
        /// </summary>
        public bool UsesConditionals = true;

        /// <summary>
        /// If <see cref="EvaluateConditionals"/> is set to true, only VDF properties 1) without any specified conditional logic or 2) conditional logic matching defined conditionals will be returned.
        /// </summary>
        public IReadOnlyList<string>? DefinedConditionals { get; set; }

        /// <summary>
        /// Sets the size of the token buffer used for deserialization.
        /// </summary>
        public int MaximumTokenSize = 4096;
    }
}
