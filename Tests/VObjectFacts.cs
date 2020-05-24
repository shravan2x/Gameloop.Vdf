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

        [Fact]
        public void DeepEqualsSucceedsCorrectly()
        {
            VObject obj1 = new VObject
            {
                new VProperty("key1", new VValue("value1")),
                new VProperty("key2", new VValue("value2")),
            };

            VObject obj2 = new VObject
            {
                new VProperty("key1", new VValue("value1")),
                new VProperty("key2", new VValue("value2")),
            };

            Assert.True(VToken.DeepEquals(obj1, obj2));
        }

        [Fact]
        public void DeepEqualsFailsCorrectly()
        {
            VObject obj1 = new VObject
            {
                new VProperty("key1", new VValue("value1")),
                new VProperty("key2", new VValue("value2")),
            };

            VObject obj2 = new VObject
            {
                new VProperty("key1", new VValue("value1")),
                new VProperty("key2", new VValue("value3")),
            };

            Assert.False(VToken.DeepEquals(obj1, obj2));
        }
    }
}
