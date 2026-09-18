# Concurrent Collections

Normal collections like List and Dictionary break when several threads change them at once. The System.Collections.Concurrent types and Channels are built to be shared safely between threads.

## Which one?

| Type | Behaves like | Good for |
|---|---|---|
| `ConcurrentDictionary<K, V>` | `Dictionary` | Shared caches, counting by key |
| `ConcurrentQueue<T>` | `Queue` (FIFO) | Work lists in order |
| `ConcurrentStack<T>` | `Stack` (LIFO) | Undo-style or "most recent first" work |
| `ConcurrentBag<T>` | an unordered bag | Collecting results when order doesn't matter |
| `BlockingCollection<T>` | a queue that makes threads **wait** | Producer / consumer with threads |
| `Channel<T>` | a queue you can `await` | Producer / consumer with async code |

## Use the Try methods

Checking and then acting is a race: another thread can take the item between your two steps.

```csharp
// Wrong: someone else may dequeue between Count and Dequeue
if (queue.Count > 0) { var item = queue.Dequeue(); }

// Right: one atomic step
if (concurrentQueue.TryDequeue(out var item)) { /* use item */ }
```

## Producer / consumer

One part of the program **produces** work and another **consumes** it, connected by a queue. A **bounded** queue (with a maximum size) makes a fast producer wait, which is called **back-pressure**, so memory can't explode.

```csharp
var channel = Channel.CreateBounded<int>(capacity: 10);
await channel.Writer.WriteAsync(42);            // producer
await foreach (int item in channel.Reader.ReadAllAsync())
{
    Console.WriteLine(item);                    // consumer
}
```

When the producer is done, it calls `Complete()` (or `CompleteAdding()`) so the consumers' loops can end.
