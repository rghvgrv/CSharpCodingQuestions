namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 5, Title = "Many Readers, One Writer (ReaderWriterLockSlim)", Level = Medium, Problem = """
    A settings cache is read constantly by many threads and changed only rarely. Make it thread-safe without making the readers wait for each other.
    """)]
public static class ReadMostlyCache
{
    [Approach(Name = "Plain lock", Idea = """
        One lock for everything. Correct, but readers block each other too, even though reading at the same time is perfectly safe.
        """)]
    public class LockedCache
    {
        private readonly Dictionary<string, string> values = new();
        private readonly object gate = new object();

        public string? Read(string key)
        {
            lock (gate)
            {
                Thread.Sleep(1);   // pretend reading takes a moment
                return values.GetValueOrDefault(key);
            }
        }

        public void Write(string key, string value)
        {
            lock (gate)
            {
                values[key] = value;
            }
        }
    }

    [Approach(Name = "ReaderWriterLockSlim", Idea = """
        Two kinds of lock:

        - **Read lock**: any number of threads can hold it together.
        - **Write lock**: only one thread, and only when no one is reading.

        When reads are much more common than writes, readers almost never wait.
        """)]
    public class ReadWriteCache
    {
        private readonly Dictionary<string, string> values = new();
        private readonly ReaderWriterLockSlim rwLock = new();

        public string? Read(string key)
        {
            rwLock.EnterReadLock();
            try
            {
                Thread.Sleep(1);   // pretend reading takes a moment
                return values.GetValueOrDefault(key);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public void Write(string key, string value)
        {
            rwLock.EnterWriteLock();
            try
            {
                values[key] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }

    public static void Demo()
    {
        var locked = new LockedCache();
        var readWrite = new ReadWriteCache();
        locked.Write("theme", "dark");
        readWrite.Write("theme", "dark");

        long lockedMs = TimeReads(() => locked.Read("theme"));
        long readWriteMs = TimeReads(() => readWrite.Read("theme"));

        Print("Plain lock: 8 threads × 5 reads", $"{lockedMs} ms (readers take turns)");
        Print("ReaderWriterLockSlim: same reads", $"{readWriteMs} ms (readers share)");
        Print("Readers were faster together", readWriteMs < lockedMs, expected: true);

        readWrite.Write("theme", "light");
        Print("Read after a write", readWrite.Read("theme"), expected: "light");
    }

    private static long TimeReads(Func<string?> read)
    {
        var clock = Stopwatch.StartNew();
        var threads = Enumerable.Range(0, 8).Select(_ => new Thread(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                read();
            }
        })).ToList();
        threads.ForEach(thread => thread.Start());
        threads.ForEach(thread => thread.Join());
        return clock.ElapsedMilliseconds;
    }
}
