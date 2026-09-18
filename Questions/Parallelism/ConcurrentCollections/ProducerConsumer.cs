namespace CSharpCodingQuestions.Questions.Parallelism.ConcurrentCollections;

[Question(Order = 3, Title = "Producer and Consumers (BlockingCollection)", Level = Medium, Problem = """
    One thread **produces** 20 jobs; two threads **consume** them. The buffer between them may hold at most 5 jobs,
    so a fast producer has to wait. When the producer is finished, the consumers must stop cleanly.
    """)]
public static class ProducerConsumer
{
    [Approach(Name = "ConcurrentQueue + Polling", Idea = """
        Consumers loop, trying to take a job and sleeping a little when the queue is empty, until a "producer is done" flag is set.
        It works, but consumers wake up for nothing, and there's no limit on how full the queue can get.
        """)]
    public static List<int> RunWithPolling(int jobCount)
    {
        var queue = new ConcurrentQueue<int>();
        var consumed = new ConcurrentBag<int>();
        bool producerDone = false;

        var consumers = Enumerable.Range(0, 2).Select(_ => new Thread(() =>
        {
            while (true)
            {
                if (queue.TryDequeue(out int job))
                {
                    consumed.Add(job);
                }
                else if (Volatile.Read(ref producerDone))
                {
                    if (queue.IsEmpty)
                    {
                        return;
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        })).ToList();
        consumers.ForEach(consumer => consumer.Start());

        for (int job = 1; job <= jobCount; job++)
        {
            queue.Enqueue(job);
        }
        Volatile.Write(ref producerDone, true);
        consumers.ForEach(consumer => consumer.Join());
        return consumed.Order().ToList();
    }

    [Approach(Name = "BlockingCollection", Idea = """
        `BlockingCollection` handles all the waiting for you:

        - `Add` **blocks** while the buffer is full (`boundedCapacity: 5`), which slows down a fast producer. This is called back-pressure.
        - `GetConsumingEnumerable()` **blocks** while the buffer is empty and hands out each job to exactly one consumer.
        - `CompleteAdding()` says "no more jobs": once the buffer is empty, the consumers' loops simply end.
        """)]
    public static List<int> RunWithBlockingCollection(int jobCount)
    {
        using var buffer = new BlockingCollection<int>(boundedCapacity: 5);
        var consumed = new ConcurrentBag<int>();

        var consumers = Enumerable.Range(0, 2).Select(_ => new Thread(() =>
        {
            foreach (int job in buffer.GetConsumingEnumerable())
            {
                Thread.Sleep(2);   // pretend to work
                consumed.Add(job);
            }
        })).ToList();
        consumers.ForEach(consumer => consumer.Start());

        for (int job = 1; job <= jobCount; job++)
        {
            buffer.Add(job);
        }
        buffer.CompleteAdding();
        consumers.ForEach(consumer => consumer.Join());
        return consumed.Order().ToList();
    }

    public static void Demo()
    {
        List<int> expected = Enumerable.Range(1, 20).ToList();
        Print("Polling: every job consumed once", RunWithPolling(20), expected: expected);
        Print("BlockingCollection: every job consumed once", RunWithBlockingCollection(20), expected: expected);
    }
}
