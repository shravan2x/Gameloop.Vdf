using Gameloop.Vdf.Linq;
using Xunit;

namespace Tests
{
    public class VPropertyFacts
    {
        [Fact]
        public void DeepCloneWorksCorrectly()
        {
            VProperty original = new VProperty("key1", new VObject
            {
                new VProperty("key2", new VValue("value2")),
            });

            VProperty clone = original.DeepClone() as VProperty;
            clone.Value = new VValue("value3");

            Assert.True(original.Value is VObject);
        }
    }
}
