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

        [Fact]
        public void CommentsDeserializeCorrectly()
        {
            const string vdf = @"
                // Comment type A (at the start of the file)
                ""root""
                {
                    // Comment type B (as a child to an object)
                    key1 ""value1""
                    ""key2"" // Comment type C (to the right of a property name)
                    {
                        ""key3"" ""value3"" // Comment type D (to the right of a property value)
                    }
                }
                // Comment type E (at the end of the file)
            ";
            VProperty result = VdfConvert.Deserialize(vdf);

            VProperty expected = new VProperty("root", new VObject
            {
                VValue.CreateComment(" Comment type B (as a child to an object)"),
                new VProperty("key1", new VValue("value1")),
                new VProperty("key2", new VObject
                {
                    new VProperty("key3", new VValue("value3")),
                    VValue.CreateComment(" Comment type D (to the right of a property value)"),
                }),
            });

            Assert.True(VToken.DeepEquals(result, expected));
        }

        [Fact]
        public void DoubleSlashInValueDeserializesCorrectly()
        {
            const string vdf = @"
                ""root""
                {
                    ""key1"" ""//""
                }
            ";
            VProperty result = VdfConvert.Deserialize(vdf);

            VProperty expected = new VProperty("root", new VObject
            {
                new VProperty("key1", new VValue("//")),
            });

            Assert.True(VToken.DeepEquals(result, expected));
        }
    }
}
