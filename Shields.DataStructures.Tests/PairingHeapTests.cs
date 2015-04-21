using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shields.DataStructures.Tests
{
    [TestClass]
    public class PairingHeapTests : PriorityQueueTests
    {
        protected override IPriorityQueue<int, int> CreatePriorityQueue()
        {
            return new PairingHeap<int, int>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorOnNullComparerThrows()
        {
            new PairingHeap<int, int>(null);
        }
    }
}
