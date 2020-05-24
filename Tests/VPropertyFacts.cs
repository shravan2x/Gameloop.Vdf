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

        [Fact]
        public void DeepEqualsSucceedsCorrectly()
        {
            VProperty prop1 = new VProperty("key1", new VValue("value1"));
            VProperty prop2 = new VProperty("key1", new VValue("value1"));

            Assert.True(VToken.DeepEquals(prop1, prop2));
        }

        [Fact]
        public void DeepEqualsFailsCorrectly()
        {
            VProperty prop1 = new VProperty("key1", new VValue("value1"));
            VProperty prop2 = new VProperty("key2", new VValue("value1"));
            VProperty prop3 = new VProperty("key1", new VValue("value2"));

            Assert.False(VToken.DeepEquals(prop1, prop2));
            Assert.False(VToken.DeepEquals(prop1, prop3));
        }
    }
}
