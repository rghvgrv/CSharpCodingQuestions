namespace CSharpCodingQuestions.Questions.Parallelism.ConcurrentCollections;

[Question(Order = 2, Title = "A Work Queue Shared by Many Threads", Level = Easy, Problem = """
    10,000 jobs sit in a queue. 4 worker threads take jobs until the queue is empty. Every job must be done **exactly once**.
    """)]
public static class SharedWorkQueue
{
    [Approach(Name = "Queue + lock", Idea = """
        A normal `Queue` needs a lock. And "check `Count`, then `Dequeue`" must happen **inside the same lock**:
        otherwise two workers can both see 1 item left and both try to take it.
        """)]
    public static long ProcessWithLock(int jobCount)
    {
        var queue = new Queue<int>(Enumerable.Range(1, jobCount));
        object gate = new object();
        long total = 0;

        var workers = Enumerable.Range(0, 4).Select(_ => new Thread(() =>
        {
            while (true)
            {
                int job;
                lock (gate)
                {
                    if (queue.Count == 0)
                    {
                        return;
                    }
                    job = queue.Dequeue();
                }
                Interlocked.Add(ref total, job);
            }
        })).ToList();
        workers.ForEach(worker => worker.Start());
        workers.ForEach(worker => worker.Join());
        return total;
    }

    [Approach(Name = "ConcurrentQueue", Idea = """
        `ConcurrentQueue.TryDequeue` checks **and** takes in one safe step: it returns `false` when the queue is empty.
        No lock is needed.

        The family: `ConcurrentQueue` (first in, first out), `ConcurrentStack` (last in, first out) and `ConcurrentBag` (no order, fastest).
        """)]
    public static long ProcessWithConcurrentQueue(int jobCount)
    {
        var queue = new ConcurrentQueue<int>(Enumerable.Range(1, jobCount));
        long total = 0;

        var workers = Enumerable.Range(0, 4).Select(_ => new Thread(() =>
        {
            while (queue.TryDequeue(out int job))
            {
                Interlocked.Add(ref total, job);
            }
        })).ToList();
        workers.ForEach(worker => worker.Start());
        workers.ForEach(worker => worker.Join());
        return total;
    }

    public static void Demo()
    {
        long expected = 10_000L * 10_001 / 2;   // 1 + 2 + … + 10,000
        Print("Queue + lock: sum of all jobs", ProcessWithLock(10_000), expected: expected);
        Print("ConcurrentQueue: sum of all jobs", ProcessWithConcurrentQueue(10_000), expected: expected);
    }
}
