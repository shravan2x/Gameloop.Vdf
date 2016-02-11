namespace Gameloop.Vdf
{
    public class VdfSerializerSettings
    {
        public static readonly VdfSerializerSettings Default = new VdfSerializerSettings();

        /// <summary>
        /// Determines whether the parser should translate escape sequences (/n, /t, etc.).
        /// </summary>
        public bool UsesEscapeSequences = false;

        /// <summary>
        /// Determines whether the parser should evaluate conditional blocks ([$WINDOWS], etc.).
        /// </summary>
        public bool UsesConditionals = true;
    }
}
