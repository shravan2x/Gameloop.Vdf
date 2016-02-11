using System;
using System.IO;

namespace Gameloop.Vdf
{
    public class VdfTextWriter : VdfWriter
    {
        public VdfTextWriter(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
        }
    }
}
