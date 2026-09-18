namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_06, "Design a Bounded Blocking Queue", Medium,
"Implement a thread-safe queue with a capacity: Enqueue blocks when full, Dequeue blocks when empty. Build it yourself with Monitor.Wait / PulseAll. (LeetCode 1188)")]
public static class BoundedBlockingQueue
{
    // One lock guards the queue. Waiters sleep with Monitor.Wait (which releases the lock)
    // and re-check their condition in a while loop after waking.
    public class MyBlockingQueue<T>(int capacity)
    {
        readonly Queue<T> items = new();
        readonly object gate = new();
        public int MaxSeenCount { get; private set; }

        public void Enqueue(T item)
        {
            lock (gate)
            {
                while (items.Count == capacity) Monitor.Wait(gate);
                items.Enqueue(item);
                MaxSeenCount = Math.Max(MaxSeenCount, items.Count);
                Monitor.PulseAll(gate); // a consumer may be waiting
            }
        }

        public T Dequeue()
        {
            lock (gate)
            {
                while (items.Count == 0) Monitor.Wait(gate);
                T item = items.Dequeue();
                Monitor.PulseAll(gate); // a producer may be waiting
                return item;
            }
        }
    }

    public static void Run()
    {
        var q = new MyBlockingQueue<int>(3);
        long consumed = 0;
        var producers = Enumerable.Range(0, 3).Select(p => new Thread(() =>
        {
            for (int i = 1; i <= 100; i++) q.Enqueue(i);
        })).ToList();
        var consumers = Enumerable.Range(0, 2).Select(_ => new Thread(() =>
        {
            for (int i = 0; i < 150; i++) Interlocked.Add(ref consumed, q.Dequeue());
        })).ToList();

        var all = producers.Concat(consumers).ToList();
        all.ForEach(t => t.Start());
        all.ForEach(t => t.Join());

        Check("Sum consumed = 3 × (1 + … + 100)", consumed, 3L * 5050);
        Check("Queue never exceeded capacity 3", q.MaxSeenCount <= 3, true);
    }
}
