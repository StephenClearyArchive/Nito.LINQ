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
        public void LinkedListNodes_EmptyList_ReturnsEmptySequence()
        {
            var list = new LinkedList<int>();
            var result = list.Nodes();
            Assert.IsTrue(result.SequenceEqual(new LinkedListNode<int>[] { }), "Nodes on an empty list should result in an empty sequence.");
        }

        [TestMethod]
        public void LinkedListNodes_SingleElementList_ReturnsSingleElementSequence()
        {
            var list = new LinkedList<int>();
            list.AddLast(13);
            var result = list.Nodes();
            Assert.IsTrue(result.Select(x => x.Value).SequenceEqual(new[] { 13 }), "Nodes on a single-element list should result in a single-element sequence.");
        }

        [TestMethod]
        public void LinkedListNodes_TwoElementList_ReturnsTwoElementSequence()
        {
            var list = new LinkedList<int>();
            list.AddLast(13);
            list.AddLast(17);
            var result = list.Nodes();
            Assert.IsTrue(result.Select(x => x.Value).SequenceEqual(new[] { 13, 17 }), "Nodes on a two-element list should result in a two-element sequence.");
        }

        [TestMethod]
        public void LinkedListNodes_AllowsRemoval()
        {
            var list = new LinkedList<int>();
            List<int> result = new List<int>();
            list.AddLast(13);
            list.AddLast(17);
            var nodes = list.Nodes().Do(x => { if (x.Value == 13) list.Remove(x); });
            Assert.IsTrue(nodes.Select(x => x.Value).SequenceEqual(new[] { 13, 17 }), "Nodes on a two-element list should result in a two-element sequence.");
            Assert.IsTrue(list.Nodes().Select(x => x.Value).SequenceEqual(new[] { 17 }), "Nodes should allow removal of the current element.");
        }
    }
}
