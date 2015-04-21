# `Shields.DataStructures` [![Build status](https://ci.appveyor.com/api/projects/status/wtr06q9vbl5krel0/branch/master?svg=true)](https://ci.appveyor.com/project/timothy-shields/data-structures/branch/master)
Data structures for .NET



`Shields.DataStructures` provides the following data structures.

# Priority Queue
## Interface
A priority queue is a data structure that stores a collection of key/value pairs. The key in each pair represents a priority, with a lesser key meaning a higher priority.

```C#
interface IPriorityQueue<TKey, TValue>
{
    IComparer<TKey> Comparer { get; }
    int Count { get; }
    IEnumerable<IPriorityQueueHandle<TKey, TValue>> Handles { get; }
    IPriorityQueueHandle<TKey, TValue> Min { get; }
    IPriorityQueueHandle<TKey, TValue> Add(TKey key, TValue value);
    void Remove(IPriorityQueueHandle<TKey, TValue> handle);
    void UpdateKey(IPriorityQueueHandle<TKey, TValue> handle, TKey key);
}
    
interface IPriorityQueueHandle<TKey, TValue>
{
    TKey Key { get; }
    TValue Value { get; }
    bool IsActive { get; }
}
```

## Implementation
[`PairingHeap<TKey, TValue>`](http://en.wikipedia.org/wiki/Pairing_heap) is an efficient implementation of `IPriorityQueue<TKey, TValue>`. If the current number of items is N, it has O(N) space complexity and the following time complexity for its methods:

<table>
  <tr><th>Space Complexity</th></tr>
  <tr><td>O(N)</td></tr>
</table>

<table>
  <tr><th>Operation</th><th>Time Complexity</th><th>Amortized?</th></tr>
  <tr><td>Count</td><td>O(1)</td><td>&cross;</td></tr>
  <tr><td>Min</td><td>O(1)</td><td>&cross;</td></tr>
  <tr><td>Add</td><td>O(1)</td><td>&cross;</td></tr>
  <tr><td>Remove</td><td>O(log N)</td><td>&check;</td></tr>
  <tr><td>UpdateKey</td><td>O(log N)</td><td>&check;</td></tr>
</table>
