using System;
using System.Collections.Generic;

namespace Shields.DataStructures
{
    /// <summary>
    /// Represents a collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public interface IPriorityQueue<TKey, TValue>
    {
        /// <summary>
        /// The comparer that defines the order of keys.
        /// </summary>
        IComparer<TKey> Comparer { get; }

        /// <summary>
        /// The number of items in the priority queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The collection of handles in the priority queue in an arbitrary order.
        /// </summary>
        IEnumerable<IPriorityQueueHandle<TKey, TValue>> Handles { get; }

        /// <summary>
        /// Gets a handle with a minimal key.
        /// If the priority queue is empty, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <returns>A handle with a minimal key.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the priority queue is empty.</exception>
        IPriorityQueueHandle<TKey, TValue> GetMin();

        /// <summary>
        /// Adds a key/value pair to the priority queue.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The handle for the inserted key/value pair.</returns>
        IPriorityQueueHandle<TKey, TValue> Add(TKey key, TValue value);

        /// <summary>
        /// Removes a key/value pair from the priority queue.
        /// If the given handle is not active, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="handle">The handle for the key/value pair to remove.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the handle is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the handle is not active or if the handle belongs to a different priority queue.</exception>
        void Remove(IPriorityQueueHandle<TKey, TValue> handle);

        /// <summary>
        /// Updates the key for a key/value pair that is currently in the priority queue.
        /// If the given handle is not active, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="handle">The handle for the key/value pair.</param>
        /// <param name="key">The current key is replaced with this value.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the handle is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the handle is not active or if the handle belongs to a different priority queue.</exception>
        void UpdateKey(IPriorityQueueHandle<TKey, TValue> handle, TKey key);
    }
}
