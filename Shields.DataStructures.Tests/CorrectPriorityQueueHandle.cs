using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shields.DataStructures.Tests
{
    public class CorrectPriorityQueueHandle<TKey, TValue> : IPriorityQueueHandle<TKey, TValue>
    {
        public TKey Key { get; internal set; }

        public TValue Value { get; private set; }

        public bool IsActive { get; internal set; }

        public CorrectPriorityQueueHandle(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
