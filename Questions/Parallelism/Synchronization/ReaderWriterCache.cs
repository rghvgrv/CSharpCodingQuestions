namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_05, "ReaderWriterLockSlim: Many Readers, One Writer", Medium,
"Build a thread-safe cache that is read far more often than written. Readers should run in parallel; writers need exclusive access.")]
public static class ReaderWriterCache
{
    // A plain lock serializes readers too. ReaderWriterLockSlim lets any number of readers in together,
    // but a writer waits for readers to leave and blocks new ones while it writes.
    public class Cache
    {
        readonly ReaderWriterLockSlim rw = new();
        readonly Dictionary<string, int> data = [];
        int activeReaders, maxReaders;

        public int MaxConcurrentReaders => maxReaders;

        public int? Get(string key)
        {
            rw.EnterReadLock();
            try
            {
                int now = Interlocked.Increment(ref activeReaders);
                for (int cur = maxReaders; now > cur; cur = maxReaders) Interlocked.CompareExchange(ref maxReaders, now, cur);
                Thread.Sleep(2); // simulate a slow read
                Interlocked.Decrement(ref activeReaders);
                return data.TryGetValue(key, out int v) ? v : null;
            }
            finally { rw.ExitReadLock(); }
        }

        public void Set(string key, int value)
        {
            rw.EnterWriteLock();
            try
            {
                if (activeReaders != 0) throw new InvalidOperationException("writer ran alongside a reader!");
                data[key] = value;
            }
            finally { rw.ExitWriteLock(); }
        }
    }

    public static void Run()
    {
        var cache = new Cache();
        cache.Set("hits", 0);
        var readers = Enumerable.Range(0, 8).Select(_ => new Thread(() => { for (int i = 0; i < 20; i++) cache.Get("hits"); }));
        var writer = new Thread(() => { for (int i = 1; i <= 10; i++) { cache.Set("hits", i); Thread.Sleep(3); } });
        var all = readers.Append(writer).ToList();
        all.ForEach(t => t.Start());
        all.ForEach(t => t.Join());

        Console.WriteLine($"Max readers inside the lock at once: {cache.MaxConcurrentReaders}");
        Check("Readers overlapped (> 1 at a time)", cache.MaxConcurrentReaders > 1, true);
        Check("Final value", cache.Get("hits"), 10);
    }
}
