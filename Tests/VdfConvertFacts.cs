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
            const string vdf =
                """
                // Comment type A (at the start of the file)
                "root"
                {
                    // Comment type B (as a child to an object)
                    key1 "value1"
                    "key2" // Comment type C (to the right of a property name)
                    {
                        "key3" "value3" // Comment type D (to the right of a property value)
                    }
                }
                // Comment type E (at the end of the file)
                """;
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
            const string vdf =
                """
                "root"
                {
                    "key1" "//"
                }
                """;
            VProperty result = VdfConvert.Deserialize(vdf);

            VProperty expected = new VProperty("root", new VObject
            {
                new VProperty("key1", new VValue("//")),
            });

            Assert.True(VToken.DeepEquals(result, expected));
        }

        [Fact]
        public void ObjectWithManyPropertiesDeserializesCorrectly()
        {
            const string vdf =
                """
                "result"
                {
                	"status"	"1"
                	"items_game_url"	"http://media.steampowered.com/apps/440/scripts/items/items_game.140f75a4dad99e92cd86e7092b1516fff18786a6.txt"
                	"qualities"
                	{
                		"Normal"	"0"
                		"rarity1"	"1"
                		"rarity2"	"2"
                	}
                	"originNames"
                	{
                		"vintage"	"3"
                		"rarity3"	"4"
                		"rarity4"	"5"
                	}
                }
                """;
            VProperty result = VdfConvert.Deserialize(vdf);

            VProperty expected = new VProperty("result", new VObject
            {
                new VProperty("status", new VValue("1")),
                new VProperty("items_game_url", new VValue("http://media.steampowered.com/apps/440/scripts/items/items_game.140f75a4dad99e92cd86e7092b1516fff18786a6.txt")),
                new VProperty("qualities", new VObject()
                {
                    new VProperty("Normal", new VValue("0")),
                    new VProperty("rarity1", new VValue("1")),
                    new VProperty("rarity2", new VValue("2")),
                }),
                new VProperty("originNames", new VObject()
                {
                    new VProperty("vintage", new VValue("3")),
                    new VProperty("rarity3", new VValue("4")),
                    new VProperty("rarity4", new VValue("5")),
                })
            });

            Assert.True(VToken.DeepEquals(result, expected));
        }

        [Fact]
        public void ConditionalsDeserializeCorrectly()
        {
            const string vdf =
                """
                "root"
                {
                    "key1" "//" [!$WIN32&&!$OSX&&!$PS3]
                    "key2" "some value containing [!$WIN32&&!$OSX&&!$PS3]" [$WIN32||$OSX||$PS3]
                    "key3" "//"
                }
                """;
            VProperty result = VdfConvert.Deserialize(vdf);

            VProperty expected = new VProperty("root", new VObject
            {
                new VProperty("key1", new VValue("//"), new VConditional()
                {
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "WIN32"),
                    new VConditional.Token(VConditional.TokenType.And),
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "OSX"),
                    new VConditional.Token(VConditional.TokenType.And),
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "PS3"),
                }),
                new VProperty("key2", new VValue("some value containing [!$WIN32&&!$OSX&&!$PS3]"), new VConditional()
                {
                    new VConditional.Token(VConditional.TokenType.Constant, "WIN32"),
                    new VConditional.Token(VConditional.TokenType.Or),
                    new VConditional.Token(VConditional.TokenType.Constant, "OSX"),
                    new VConditional.Token(VConditional.TokenType.Or),
                    new VConditional.Token(VConditional.TokenType.Constant, "PS3"),
                }),
                new VProperty("key3", new VValue("//")),
            });

            Assert.True(VToken.DeepEquals(result, expected));
        }

        [Fact]
        public void ConditionalsSerializeCorrectly()
        {
            VProperty vdf = new VProperty("root", new VObject
            {
                new VProperty("key1", new VValue("//"), new VConditional()
                {
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "WIN32"),
                    new VConditional.Token(VConditional.TokenType.And),
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "OSX"),
                    new VConditional.Token(VConditional.TokenType.And),
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "PS3"),
                }),
                new VProperty("key2", new VValue("some value containing [!$WIN32&&!$OSX&&!$PS3]"), new VConditional()
                {
                    new VConditional.Token(VConditional.TokenType.Constant, "WIN32"),
                    new VConditional.Token(VConditional.TokenType.Or),
                    new VConditional.Token(VConditional.TokenType.Constant, "OSX"),
                    new VConditional.Token(VConditional.TokenType.Or),
                    new VConditional.Token(VConditional.TokenType.Constant, "PS3"),
                }),
                new VProperty("key3", new VValue("//")),
            });
            string result = VdfConvert.Serialize(vdf);

            const string expected =
                """
                "root"
                {
                	"key1" "//" [!$WIN32&&!$OSX&&!$PS3]
                	"key2" "some value containing [!$WIN32&&!$OSX&&!$PS3]" [$WIN32||$OSX||$PS3]
                	"key3" "//"
                }

                """;

            Assert.True(result == expected);
        }

        [Fact]
        public void ConditionalsEvaluateCorrectly()
        {
            const string vdf =
                """
                "root"
                {
                    "key1" "//" [!$WIN32&&!$OSX&&!$PS3]
                    "key2" "//" [$WIN32||$OSX||$PS3]
                    "key3" "//"
                    "key4" "//" [$X360]
                }
                """;
            VdfSerializerSettings settings = new VdfSerializerSettings() { UsesConditionals = true, DefinedConditionals = new string[] { "X360" } };
            VProperty result = VdfConvert.Deserialize(vdf, settings);

            VProperty expected = new VProperty("root", new VObject
            {
                new VProperty("key1", new VValue("//"), new VConditional()
                {
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "WIN32"),
                    new VConditional.Token(VConditional.TokenType.And),
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "OSX"),
                    new VConditional.Token(VConditional.TokenType.And),
                    new VConditional.Token(VConditional.TokenType.Not),
                    new VConditional.Token(VConditional.TokenType.Constant, "PS3"),
                }),
                new VProperty("key3", new VValue("//")),
                new VProperty("key4", new VValue("//"), new VConditional()
                {
                    new VConditional.Token(VConditional.TokenType.Constant, "X360"),
                }),
            });

            Assert.True(VToken.DeepEquals(result, expected));
        }
    }
}
