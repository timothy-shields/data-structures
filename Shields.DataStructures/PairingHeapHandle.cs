using System;

namespace Shields.DataStructures
{
    internal class PairingHeapHandle<TKey, TValue> : IPriorityQueueHandle<TKey, TValue>
    {
        internal readonly long pairingHeapId;
        internal bool isActive;
        internal TKey key;
        internal TValue value;
        internal PairingHeapHandle<TKey, TValue> left;
        internal PairingHeapHandle<TKey, TValue> right;
        internal PairingHeapHandle<TKey, TValue> firstChild;

        public TKey Key
        {
            get { return key; }
        }

        public TValue Value
        {
            get { return value; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        internal PairingHeapHandle(long pairingHeapId, TKey key, TValue value)
        {
            this.pairingHeapId = pairingHeapId;
            this.key = key;
            this.value = value;
        }
    }
}
