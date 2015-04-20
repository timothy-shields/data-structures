using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shields.DataStructures.Tests
{
    [TestClass]
    public class PairingHeapTests : PriorityQueueTests
    {
        protected override IPriorityQueue<int, string> CreatePriorityQueue()
        {
            return new PairingHeap<int, string>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorOnNullComparerThrows()
        {
            new PairingHeap<int, string>(null);
        }
    }
}
