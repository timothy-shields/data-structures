using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace Shields.DataStructures.Tests
{
    [TestClass]
    public abstract class PriorityQueueTests
    {
        protected abstract IPriorityQueue<int, int> CreatePriorityQueue();

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MinOnEmptyPriorityQueueThrows()
        {
            var q = CreatePriorityQueue();
            var h = q.Min;
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
            var h = q1.Add(0, 0);
            q2.Remove(h);
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
            var h = q1.Add(0, 0);
            q2.UpdateKey(h, 0);
        }

        [TestMethod]
        public void RandomSequenceOfOperationsIsCorrect()
        {
            IPriorityQueue<int, int> q1 = new CorrectPriorityQueue<int, int>();
            var h1 = new List<IPriorityQueueHandle<int, int>>();
            var q2 = CreatePriorityQueue();
            var h2 = new List<IPriorityQueueHandle<int, int>>();

            var random = new Random();
            for (int iteration = 0; iteration < 1000000; iteration++)
            {
                var action = random.Next(5);
                if (q1.Count == 0)
                {
                    action = 0;
                }
                if (action == 0)
                {
                    var key = random.Next();
                    var value = random.Next();
                    h1.Add(q1.Add(key, value));
                    h2.Add(q2.Add(key, value));
                }
                else if (action == 1)
                {
                    var i1 = random.Next(h1.Count);
                    var i2 = Enumerable.Range(0, h1.Count)
                        .First(i => h1[i1].Key == h2[i].Key
                            && h1[i1].Value == h2[i].Value);
                    var key = random.Next();
                    q1.UpdateKey(h1[i1], key);
                    q2.UpdateKey(h2[i2], key);
                }
                else
                {
                    var i1 = random.Next(h1.Count);
                    var i2 = Enumerable.Range(0, h1.Count)
                        .First(i => h1[i1].Key == h2[i].Key
                            && h1[i1].Value == h2[i].Value);
                    q1.Remove(h1[i1]);
                    q2.Remove(h2[i2]);
                    h1.RemoveAt(i1);
                    h2.RemoveAt(i2);
                }
                Assert.AreEqual(q1.Count, q2.Count);
                if (q1.Count > 0)
                {
                    Assert.AreEqual(q1.Min.Key, q2.Min.Key);
                    Assert.AreEqual(q1.Min.Value, q2.Min.Value);
                }
            }
        }
    }
}
