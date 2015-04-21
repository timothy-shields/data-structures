using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Shields.DataStructures
{
    /// <summary>
    /// A mutable pairing heap data structure. http://en.wikipedia.org/wiki/Pairing_heap
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class PairingHeap<TKey, TValue> : IPriorityQueue<TKey, TValue>
    {
        private static long nextId = 0;
        private readonly long id;
        private readonly IComparer<TKey> comparer;

        private PairingHeapHandle<TKey, TValue> root;
        private int count;

        /// <summary>
        /// Constructs an empty pairing heap using the default comparer for the key type.
        /// </summary>
        public PairingHeap() : this(Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Constructs an empty pairing heap using the specified comparer for the key type.
        /// </summary>
        /// <param name="comparer">The comparer for the key type.</param>
        public PairingHeap(IComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            this.id = Interlocked.Increment(ref nextId);
            this.comparer = comparer;
            this.root = null;
            this.count = 0;
        }

        /// <summary>
        /// The comparer that defines the order of keys.
        /// </summary>
        public IComparer<TKey> Comparer
        {
            get { return comparer; }
        }

        /// <summary>
        /// The number of items in the priority queue.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// The collection of handles in the priority queue in an arbitrary order.
        /// </summary>
        public IEnumerable<IPriorityQueueHandle<TKey, TValue>> Handles
        {
            get { return GetHandles(root); }
        }

        /// <summary>
        /// Gets a handle with a minimal key.
        /// If the priority queue is empty, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <returns>A handle with a minimal key.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the priority queue is empty.</exception>
        public IPriorityQueueHandle<TKey, TValue> Min
        {
            get
            {
                if (count == 0)
                {
                    throw new InvalidOperationException("PairingHeap is empty.");
                }
                return root;
            }
        }

        /// <summary>
        /// Adds a key/value pair to the priority queue.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The handle for the inserted key/value pair.</returns>
        public IPriorityQueueHandle<TKey, TValue> Add(TKey key, TValue value)
        {
            var handle = new PairingHeapHandle<TKey, TValue>(id, key, value);
            Add(handle);
            return handle;
        }

        /// <summary>
        /// Removes a key/value pair from the priority queue.
        /// If the given handle is not active, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="handle">The handle for the key/value pair to remove.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the handle is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the handle is not active or if the handle belongs to a different priority queue.</exception>
        public void Remove(IPriorityQueueHandle<TKey, TValue> handle)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }
            var h = handle as PairingHeapHandle<TKey, TValue>;
            if (h == null || h.pairingHeapId != id)
            {
                throw new InvalidOperationException("Tried to remove a handle from a different priority queue than that which created it.");
            }
            if (!h.IsActive)
            {
                throw new InvalidOperationException("Tried to remove an inactive handle.");
            }
            count--;
            if (h == root)
            {
                root = DeleteRoot(root);
            }
            else
            {
                SpliceOut(h);
                root = Pair(root, DeleteRoot(h));
            }
            h.isActive = false;
            h.left = null;
            h.right = null;
            h.firstChild = null;
        }

        /// <summary>
        /// Updates the key for a key/value pair that is currently in the priority queue.
        /// If the given handle is not active, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="handle">The handle for the key/value pair.</param>
        /// <param name="key">The current key is replaced with this value.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the handle is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the handle is not active or if the handle belongs to a different priority queue.</exception>
        public void UpdateKey(IPriorityQueueHandle<TKey, TValue> handle, TKey key)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }
            var h = handle as PairingHeapHandle<TKey, TValue>;
            if (h == null || h.pairingHeapId != id)
            {
                throw new InvalidOperationException("Tried to update the key of a handle of a different priority queue than that which created it.");
            }
            if (!h.IsActive)
            {
                throw new InvalidOperationException("Tried to update the key of an inactive handle.");
            }
            if (comparer.Compare(key, h.Key) <= 0)
            {
                h.key = key;
                if (h != root)
                {
                    SpliceOut(h);
                    root = Pair(root, h);
                }
            }
            else
            {
                Remove(h);
                h.key = key;
                Add(h);
            }
        }

        private IEnumerable<PairingHeapHandle<TKey, TValue>> GetHandles(PairingHeapHandle<TKey, TValue> n)
        {
            if (n == null)
            {
                yield break;
            }
            yield return n;
            for (var c = n.firstChild; c != null; c = c.right)
            {
                foreach (var h in GetHandles(c))
                {
                    yield return h;
                }
            }
        }

        private void Add(PairingHeapHandle<TKey, TValue> handle)
        {
            Debug.Assert(!handle.isActive);
            count++;
            if (root == null)
            {
                root = handle;
            }
            else
            {
                root = Pair(root, handle);
            }
            handle.isActive = true;
        }

        private PairingHeapHandle<TKey, TValue> Pair(PairingHeapHandle<TKey, TValue> n1, PairingHeapHandle<TKey, TValue> n2)
        {
            if (n1 == null)
            {
                return n2;
            }
            if (n2 == null)
            {
                return n1;
            }
            Debug.Assert(n1.left == null);
            Debug.Assert(n2.left == null);
            if (comparer.Compare(n1.key, n2.key) < 0)
            {
                var temp = n1;
                n1 = n2;
                n2 = temp;
            }
            //n1 becomes the first child of n2.
            var c = n2.firstChild;
            n1.left = n2;
            n1.right = c;
            if (c != null)
            {
                c.left = n1;
            }
            n2.firstChild = n1;
            return n2;
        }

        private static void SpliceOut(PairingHeapHandle<TKey, TValue> n)
        {
            if (n.left == null)
            {
                return;
            }
            if (n.left.firstChild == n)
            {
                //n is the first child of its parent.
                var p = n.left;
                if (n.right == null)
                {
                    //n is the only child of p.
                    p.firstChild = null;
                }
                else
                {
                    //n has a right sibling.
                    var r = n.right;
                    r.left = p;
                    p.firstChild = r;
                }
            }
            else
            {
                //n is not the first child of its parent.
                var l = n.left;
                var r = n.right;
                l.right = r;
                if (r != null)
                {
                    r.left = l;
                }
            }
            n.left = null;
            n.right = null;
        }

        private PairingHeapHandle<TKey, TValue> DeleteRoot(PairingHeapHandle<TKey, TValue> n)
        {
            Debug.Assert(n.left == null);
            if (n.firstChild == null)
            {
                return null;
            }
            if (n.firstChild.right != null)
            {
                //n has at least two children.
                var c1 = n.firstChild;
                var c2 = c1.right;
                SpliceOut(c1);
                SpliceOut(c2);
                return Pair(Pair(c1, c2), DeleteRoot(n));
            }
            //n has a single child.
            var c = n.firstChild;
            c.left = null;
            n.firstChild = null;
            return c;
        }
    }
}