# Heaps & Priority Queues

A heap always gives you the smallest (or largest) item in constant time, and adding or removing items costs only O(log n). Use it whenever you need "the next most important item".

## The idea

A **min-heap** is a tree where every parent is smaller than or equal to its children, so the smallest item is always at the top. It's stored in a plain array:

- children of index `i` are at `2i + 1` and `2i + 2`
- the parent of index `i` is at `(i - 1) / 2`

When you add an item, it **bubbles up** until its parent is smaller. When you remove the top, the last item moves to the top and **sinks down**.

## In C#

`PriorityQueue<TElement, TPriority>` is a min-heap: the lowest priority comes out first.

```csharp
var tasks = new PriorityQueue<string, int>();
tasks.Enqueue("write report", 2);
tasks.Enqueue("fix bug", 1);
tasks.Enqueue("lunch", 3);
Console.WriteLine(tasks.Dequeue());   // "fix bug"

// Max-heap: reverse the comparison
var maxHeap = new PriorityQueue<int, int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
```

## Cost

| Operation | Cost |
|---|---|
| Look at the top (`Peek`) | `O(1)` |
| Add (`Enqueue`) | `O(log n)` |
| Remove the top (`Dequeue`) | `O(log n)` |
| Build from `n` items | `O(n)` |

## Classic uses

- **Top K** items: keep a heap of size `k`.
- **Merging** many sorted lists.
- **Running median**: two heaps, one for each half.
- **Dijkstra's shortest path** and scheduling "what runs next".
