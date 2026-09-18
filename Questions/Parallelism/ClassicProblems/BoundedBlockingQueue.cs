namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 6, Title = "Build a Bounded Blocking Queue", Level = Medium, Problem = """
    Build your own thread-safe queue with a maximum size: `Enqueue` waits while the queue is full, and `Dequeue` waits while it's empty.
    (This is what `BlockingCollection` does inside. LeetCode 1188)
    """)]
public static class BoundedBlockingQueue
{
    [Approach(Name = "Lock + Sleep and Retry", Idea = """
        Try inside a lock; if the queue is full (or empty), leave the lock, sleep a little, and try again.
        It works, but threads keep waking up to check, and there's a delay after the queue changes.
        """)]
    public class PollingQueue(int capacity)
    {
        private readonly Queue<int> items = new();
        private readonly object gate = new object();

        public void Enqueue(int item)
        {
            while (true)
            {
                lock (gate)
                {
                    if (items.Count < capacity)
                    {
                        items.Enqueue(item);
                        return;
                    }
                }
                Thread.Sleep(1);
            }
        }

        public int Dequeue()
        {
            while (true)
            {
                lock (gate)
                {
                    if (items.Count > 0)
                    {
                        return items.Dequeue();
                    }
                }
                Thread.Sleep(1);
            }
        }
    }

    [Approach(Name = "Monitor.Wait and PulseAll", Idea = """
        Sleep until something **changes**, instead of checking on a timer:

        - Full? `Monitor.Wait(gate)` releases the lock and sleeps. A `Dequeue` will wake it with `PulseAll`.
        - Empty? Same thing, woken up by an `Enqueue`.
        - After every change, `Monitor.PulseAll(gate)` wakes the waiting threads so they can check again.

        The `while` loops matter: a woken thread must re-check, because another thread may have grabbed the space or item first.
        """)]
    public class BlockingQueue(int capacity)
    {
        private readonly Queue<int> items = new();
        private readonly object gate = new object();

        public int MaxSizeSeen { get; private set; }

        public void Enqueue(int item)
        {
            lock (gate)
            {
                while (items.Count == capacity)
                {
                    Monitor.Wait(gate);
                }
                items.Enqueue(item);
                MaxSizeSeen = Math.Max(MaxSizeSeen, items.Count);
                Monitor.PulseAll(gate);
            }
        }

        public int Dequeue()
        {
            lock (gate)
            {
                while (items.Count == 0)
                {
                    Monitor.Wait(gate);
                }
                int item = items.Dequeue();
                Monitor.PulseAll(gate);
                return item;
            }
        }
    }

    public static void Demo()
    {
        var queue = new BlockingQueue(capacity: 3);
        long total = 0;
        var producers = Enumerable.Range(0, 3).Select(_ => new Thread(() =>
        {
            for (int i = 1; i <= 100; i++)
            {
                queue.Enqueue(i);
            }
        }));
        var consumers = Enumerable.Range(0, 2).Select(_ => new Thread(() =>
        {
            for (int i = 0; i < 150; i++)
            {
                Interlocked.Add(ref total, queue.Dequeue());
            }
        }));
        var all = producers.Concat(consumers).ToList();
        all.ForEach(thread => thread.Start());
        all.ForEach(thread => thread.Join());

        Print("3 producers × (1 + … + 100) all consumed", total, expected: 3L * 5050);
        Print("Queue never held more than 3", queue.MaxSizeSeen <= 3, expected: true);

        var polling = new PollingQueue(capacity: 2);
        var producer = new Thread(() =>
        {
            for (int i = 1; i <= 5; i++)
            {
                polling.Enqueue(i);
            }
        });
        producer.Start();
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            sum += polling.Dequeue();
        }
        producer.Join();
        Print("Polling version also works", sum, expected: 15);
    }
}
