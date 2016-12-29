using System.Globalization;
using System.IO;

namespace Gameloop.Vdf
{
    public abstract class VToken
    {
        public VToken Parent { get; set; }
        public VToken Previous { get; set; }
        public VToken Next { get; set; }

        public abstract void WriteTo(VdfWriter writer);

        public override string ToString()
        {
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                VdfTextWriter vdfTextWriter = new VdfTextWriter(stringWriter);
                WriteTo(vdfTextWriter);

                return stringWriter.ToString();
            }
        }
    }
}
