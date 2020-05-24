using Gameloop.Vdf.Linq;
using Xunit;

namespace Tests
{
    public class VValueFacts
    {
        [Fact]
        public void DeepCloneWorksCorrectly()
        {
            VValue original = new VValue("value1");

            VValue clone = original.DeepClone() as VValue;
            clone.Value = "value2";

            Assert.True(original.Value.Equals("value1"));
        }
    }
}
