using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using System;
using Xunit;

namespace Tests
{
    public class VdfConvertFacts
    {
        [Fact]
        public void EmptyStringThrowsException()
        {
            Assert.Throws<VdfException>(() => VdfConvert.Deserialize(String.Empty));
        }
    }
}
