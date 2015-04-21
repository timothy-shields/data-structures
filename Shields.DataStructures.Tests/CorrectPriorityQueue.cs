using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Shields.DataStructures.Tests
{
    public class CorrectPriorityQueue<TKey, TValue> : IPriorityQueue<TKey, TValue>
    {
        private readonly IComparer<TKey> comparer = Comparer<TKey>.Default;
        private readonly List<CorrectPriorityQueueHandle<TKey, TValue>> handles = new List<CorrectPriorityQueueHandle<TKey, TValue>>();

        public IComparer<TKey> Comparer
        {
            get { return comparer; }
        }

        public int Count
        {
            get { return handles.Count; }
        }

        public IEnumerable<IPriorityQueueHandle<TKey, TValue>> Handles
        {
            get { return handles.AsReadOnly(); }
        }

        public IPriorityQueueHandle<TKey, TValue> Min
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException();
                }
                return handles.MinBy(h => h.Key, comparer).First();
            }
        }

        public IPriorityQueueHandle<TKey, TValue> Add(TKey key, TValue value)
        {
            var handle = new CorrectPriorityQueueHandle<TKey, TValue>(key, value);
            handles.Add(handle);
            handle.IsActive = true;
            return handle;
        }

        public void Remove(IPriorityQueueHandle<TKey, TValue> handle)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }
            if (!handle.IsActive)
            {
                throw new InvalidOperationException();
            }
            var h = (CorrectPriorityQueueHandle<TKey, TValue>)handle;
            handles.Remove(h);
            h.IsActive = false;
        }

        public void UpdateKey(IPriorityQueueHandle<TKey, TValue> handle, TKey key)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }
            if (!handle.IsActive)
            {
                throw new InvalidOperationException();
            }
            var h = (CorrectPriorityQueueHandle<TKey, TValue>)handle;
            h.Key = key;
        }
    }
}