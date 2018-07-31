namespace Gameloop.Vdf.Utilities
{
    internal static class MiscellaneousUtils
    {
        public static string ToString(object value)
        {
            if (value == null)
            {
                return "{null}";
            }

            return (value is string) ? @"""" + value.ToString() + @"""" : value.ToString();
        }
    }
}
