using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Shields.DataStructures
{
    [DebuggerDisplay("Count = {Count}")]
    public class PairingHeap<TKey, TValue> : IPriorityQueue<TKey, TValue>
    {
        private static long nextId = 0;
        private readonly long id;
        private PairingHeapHandle<TKey, TValue> root;
        private int count;
        private readonly IComparer<TKey> comparer;

        public PairingHeap() : this(Comparer<TKey>.Default)
        {
        }

        public PairingHeap(IComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            this.id = Interlocked.Increment(ref nextId);
            this.root = null;
            this.count = 0;
            this.comparer = comparer;
        }

        public IComparer<TKey> Comparer
        {
            get { return comparer; }
        }

        public int Count
        {
            get { return count; }
        }

        public IEnumerable<IPriorityQueueHandle<TKey, TValue>> Handles
        {
            get { return GetHandles(root); }
        }

        public IPriorityQueueHandle<TKey, TValue> GetMin()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("PairingHeap is empty.");
            }
            return root;
        }

        public IPriorityQueueHandle<TKey, TValue> Add(TKey key, TValue value)
        {
            var handle = new PairingHeapHandle<TKey, TValue>(id, key, value);
            Add(handle);
            return handle;
        }

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