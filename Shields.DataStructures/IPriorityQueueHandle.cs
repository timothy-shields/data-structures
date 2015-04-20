using System;

namespace Shields.DataStructures
{
    /// <summary>
    /// Represents a key/value pair in a priority queue.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public interface IPriorityQueueHandle<TKey, TValue>
    {
        /// <summary>
        /// The key for this handle.
        /// </summary>
        TKey Key { get; }

        /// <summary>
        /// The value for this handle.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Is this handle still in the priority queue that created it?
        /// </summary>
        bool IsActive { get; }
    }
}
