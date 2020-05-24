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

        [Fact]
        public void DeepEqualsSucceedsCorrectly()
        {
            VValue val1 = new VValue("value1");
            VValue val2 = new VValue("value1");

            Assert.True(VToken.DeepEquals(val1, val2));
        }

        [Fact]
        public void DeepEqualsFailsCorrectly()
        {
            VValue val1 = new VValue("value1");
            VValue val2 = new VValue("value2");

            Assert.False(VToken.DeepEquals(val1, val2));
        }
    }
}
