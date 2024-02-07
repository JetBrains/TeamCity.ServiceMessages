

namespace JetBrains.TeamCity.ServiceMessages.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ServiceMessageReplacementsTest
    {
        // Simple
        [TestCase("", "")]
        [TestCase("!@#$%^&*", "!@#$%^&*")]
        [TestCase("a", "a")]
        [TestCase("Abc", "Abc")]

        // Special
        [TestCase("|n", "\n")]
        [TestCase("|r", "\r")]
        [TestCase("|]", "]")]
        [TestCase("|[", "[")]
        [TestCase("|'", "'")]
        [TestCase("||", "|")]
        [TestCase("|x", "\u0085")]
        [TestCase("|l", "\u2028")]
        [TestCase("|p", "\u2029")]

        [TestCase("aaa|nbbb", "aaa\nbbb")]
        [TestCase("aaa|nbbb||", "aaa\nbbb|")]
        [TestCase("||||", "||")]

        // Unicode
        [TestCase("|0x00bf", "\u00bf")]
        [TestCase("|0x00bfaaa", "\u00bfaaa")]
        [TestCase("bb|0x00bfaaa", "bb\u00bfaaa")]
        public void ShouldDecodeAndEncodeWhenUnicode(string textFormServiceMessage, string actualText)
        {
            Assert.AreEqual(actualText, ServiceMessageReplacements.Decode(textFormServiceMessage));
            Assert.AreEqual(textFormServiceMessage, ServiceMessageReplacements.Encode(actualText));
        }

        // Invalid special
        [TestCase("|z", "?")]
        [TestCase("|", "?")]

        // Invalid unicode
        [TestCase("|0", "?")]
        [TestCase("|0x", "?")]
        [TestCase("|0x0", "?")]
        [TestCase("|0x0b", "?")]
        [TestCase("|0x00b", "?")]
        [TestCase("|0x00bg", "?")]
        [TestCase("aaa|0x00b", "aaa?")]
        [TestCase("aaa|0x00fkccc", "aaa?ccc")]

        public void ShouldDecodeWhenInvalidDecodedText(string textFormServiceMessage, string actualText)
        {
            Assert.AreEqual(actualText, ServiceMessageReplacements.Decode(textFormServiceMessage));
        }
    }
}