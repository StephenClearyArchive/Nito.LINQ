using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito;

namespace UnitTests
{
    public partial class Tests
    {
        [TestMethod]
        public void String_Join_OnEmptySequence_ReturnsEmptyString()
        {
            string result = new List<string>().Join(",");
            Assert.AreEqual(string.Empty, result, "Join on an empty string sequence should result in an empty string");
        }

        [TestMethod]
        public void String_Join_OnSingleSequence_DoesNotUseSeparator()
        {
            string result = new List<string> { "test" }.Join(",");
            Assert.AreEqual("test", result, "Join on a string sequence of a single string should result in just that string");
        }

        [TestMethod]
        public void String_Join_OnSequence_UsesSeparator()
        {
            string result = new List<string> { "test1", "test2" }.Join(", ");
            Assert.AreEqual("test1, test2", result, "Join on a string sequence should use the separator string correctly");
        }

        [TestMethod]
        public void String_Join_WithourSeparator_JustConcatenates()
        {
            string result = new List<string> { "test1", "test2" }.Join();
            Assert.AreEqual("test1test2", result, "Join on a string sequence should concatenate in the absence of a separator string");
        }
    }
}
