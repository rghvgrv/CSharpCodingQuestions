namespace CodingQuestions.Parallelism.ConcurrentCollections;

[Q(3_05_02, "ConcurrentQueue, ConcurrentStack & ConcurrentBag", Easy,
"Add items from many threads into each concurrent collection and take them out safely with the Try* methods. When would you pick each one?")]
public static class ConcurrentQueueStackBag
{
    // Queue = FIFO, Stack = LIFO, Bag = no order (fastest when the same thread adds and takes).
    // No Count-then-Dequeue: another thread can grab the item in between. Use TryDequeue / TryPop / TryTake.
    public static void Run()
    {
        var queue = new ConcurrentQueue<int>();
        var stack = new ConcurrentStack<int>();
        var bag = new ConcurrentBag<int>();
        Parallel.For(0, 10_000, i => { queue.Enqueue(i); stack.Push(i); bag.Add(i); });
        Check("Counts", (queue.Count, stack.Count, bag.Count), (10_000, 10_000, 10_000));

        long sum = 0;
        Parallel.For(0, 4, _ =>
        {
            while (queue.TryDequeue(out int x)) Interlocked.Add(ref sum, x);
        });
        Check("4 threads drained the queue, sum", sum, Enumerable.Range(0, 10_000).Sum(x => (long)x));
        Check("Queue empty", queue.IsEmpty, true);

        var single = new ConcurrentStack<int>();
        single.PushRange([1, 2, 3]);
        Check("Stack TryPop (LIFO)", single.TryPop(out int top) ? top : -1, 3);

        var fifo = new ConcurrentQueue<string>(["first", "second"]);
        Check("Queue TryPeek (FIFO)", fifo.TryPeek(out var head) ? head : null, "first");

        Check("Bag contains every item", bag.Order().SequenceEqual(Enumerable.Range(0, 10_000)), true);
    }
}
