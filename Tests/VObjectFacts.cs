using Gameloop.Vdf.Linq;
using Xunit;

namespace Tests
{
    public class VObjectFacts
    {
        [Fact]
        public void DeepCloneWorksCorrectly()
        {
            VObject original = new VObject
            {
                new VProperty("key1", new VValue("value1")),
            };

            VObject clone = original.DeepClone() as VObject;
            clone["key1"] = new VValue("value2");

            Assert.True(((VValue) original["key1"]).Value.Equals("value1"));
        }
    }
}
