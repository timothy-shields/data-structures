using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shields.DataStructures.Tests
{
    [TestClass]
    public abstract class PriorityQueueTests
    {
        protected abstract IPriorityQueue<int, string> CreatePriorityQueue();

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetMinOnEmptyPriorityQueueThrows()
        {
            var q = CreatePriorityQueue();
            q.GetMin();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullHandleThrows()
        {
            var q = CreatePriorityQueue();
            q.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveOnWrongInstanceThrows()
        {
            var q1 = CreatePriorityQueue();
            var q2 = CreatePriorityQueue();
            q2.Remove(q1.Add(0, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateKeyOnNullHandleThrows()
        {
            var q = CreatePriorityQueue();
            q.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateKeyOnWrongInstanceThrows()
        {
            var q1 = CreatePriorityQueue();
            var q2 = CreatePriorityQueue();
            q2.UpdateKey(q1.Add(0, null), 0);
        }
    }
}
