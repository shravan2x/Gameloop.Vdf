------------------------------------------------------------------------------
v 0.6.1			Jun 20, 2020
------------------------------------------------------------------------------

*	Fixed VdfTextReader treating slashes in quoted values as comments.

------------------------------------------------------------------------------
v 0.6.0			May 29, 2020
------------------------------------------------------------------------------

*	Added comment serialization and deserialization support. Learn more at https://github.com/shravan2x/Gameloop.Vdf/issues/18.
*	Added DeepClone method to VToken.
*	Added VToken.DeepEquals to deep compare two VTokens.
*	Added support for C# 8's nullable reference types.

BREAKING CHANGES
*	VObject.Children() now returns an IEnumerable<VToken>, rather than an IEnumerable<VProperty>. This is more in line with Json.NET and allows for comments (which are VTokens) to be returned.
*	VProperty's empty constructor has been removed. Neither Key or Value should be null. C# 8's NRT feature type-checks this.
*	VObject's IDictionary<string, VToken>.this[string key] indexer now throws a KeyNotFoundException when the key isn't found. Note that that VObject's regular this[string key] indexer still returns null when the key isn't found.


------------------------------------------------------------------------------
v 0.5.0			Jul 30, 2016
------------------------------------------------------------------------------

*	Added VToken.Value, VToken.Value(), and other accessors.
*	Added IDictionary<string, VToken> as a superclass of VObject.
*	Moved VToken and subtypes to Gameloop.Vdf.Linq namespace.


------------------------------------------------------------------------------
v 0.4.4			Dec 01, 2017
------------------------------------------------------------------------------

*	Fixed SOE on deserializing an empty input.


------------------------------------------------------------------------------
v 0.4.3			Aug 29, 2017
------------------------------------------------------------------------------

*	Fixed NRE on serializing a null VValue.


------------------------------------------------------------------------------
v 0.4.2			Aug 28, 2017
------------------------------------------------------------------------------

*	Added targeting for .NET 4.5.


------------------------------------------------------------------------------
v 0.4.1			Aug 27, 2017
------------------------------------------------------------------------------

*	Fixed strings not being escaped during serialization.
*	Fixed bug in VObject set indexer.


------------------------------------------------------------------------------
v 0.4.0			Aug 10, 2017
------------------------------------------------------------------------------

*	Re-targeted project to .NET Standard 1.0.
*	Added VdfSerializerSettings.Common settings preset as default.
*	Fixed VdfConvert.Deserialize return type to VProperty.


------------------------------------------------------------------------------
v 0.3.0			Mar 26, 2017
------------------------------------------------------------------------------

*	Added serialization support.
*	Added dynamic property binding for VObject.
*	Fixed VdfTextReader not closing streams.


------------------------------------------------------------------------------
v 0.2.0			Mar 15, 2016
------------------------------------------------------------------------------

*	Added comment support.
*	Redesigned VdfSerializer and VdfConvert.
*	Allowed duplicate keys.


------------------------------------------------------------------------------
v 0.1.0			Mar 11, 2016
------------------------------------------------------------------------------

*	Initial release.
