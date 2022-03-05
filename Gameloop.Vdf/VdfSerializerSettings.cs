namespace Gameloop.Vdf
{
    public class VdfSerializerSettings
    {
        public static VdfSerializerSettings Default => new VdfSerializerSettings();
        public static VdfSerializerSettings Common => new VdfSerializerSettings
        {
            UsesEscapeSequences = true,
            UsesConditionals = true
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
        /// Sets the size of the token buffer used for deserialization.
        /// </summary>
        public int MaximumTokenSize = 4096;

        // System information
        public bool IsXBox360 = false, IsWin32 = true;
        public bool IsWindows = true, IsOSX = false, IsLinux = false, IsPosix = false;
    }
}
