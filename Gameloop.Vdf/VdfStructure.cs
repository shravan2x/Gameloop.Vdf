namespace Gameloop.Vdf
{
    public static class VdfStructure
    {
        // Format
        public const char Quote = '"', Escape = '\\';
        public const char ConditionalStart = '[', ConditionalEnd = ']';
        public const char ObjectStart = '{', ObjectEnd = '}';

        // Conditionals
        public const string ConditionalXbox360 = "$X360", ConditionalWin32 = "$WIN32";
        public const string ConditionalWindows = "$WINDOWS", ConditionalOSX = "$OSX", ConditionalLinux = "$LINUX", ConditionalPosix = "$POSIX";
    }
}
