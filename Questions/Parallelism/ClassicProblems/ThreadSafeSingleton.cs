namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_07, "Thread-Safe Singleton", Medium,
"Create exactly one instance of an expensive object even when 50 threads ask for it at the same moment. Compare the naive version, double-checked locking and Lazy<T>.")]
public static class ThreadSafeSingleton
{
    static int naiveCreated, lockedCreated, lazyCreated;

    // Broken: two threads can both see null and both create an instance.
    class Naive
    {
        static Naive? instance;
        Naive() { Interlocked.Increment(ref naiveCreated); Thread.Sleep(10); }
        public static Naive Instance => instance ??= new Naive();
    }

    // Double-checked locking: fast path without a lock once created. `volatile` stops the other threads
    // from seeing a reference to a half-constructed object.
    class Locked
    {
        static volatile Locked? instance;
        static readonly Lock gate = new();
        Locked() { Interlocked.Increment(ref lockedCreated); Thread.Sleep(10); }
        public static Locked Instance
        {
            get
            {
                if (instance == null)
                    lock (gate)
                        instance ??= new Locked();
                return instance;
            }
        }
    }

    // Idiomatic C#: Lazy<T> is thread-safe by default (ExecutionAndPublication).
    class ViaLazy
    {
        static readonly Lazy<ViaLazy> lazy = new(() => new ViaLazy());
        ViaLazy() { Interlocked.Increment(ref lazyCreated); Thread.Sleep(10); }
        public static ViaLazy Instance => lazy.Value;
    }

    // A `static readonly Instance = new()` field is also thread-safe: the runtime runs static initializers once.

    public static void Run()
    {
        // Each of these runs once per app lifetime, so counts are per process, not per click.
        using var start = new ManualResetEventSlim();
        var threads = Enumerable.Range(0, 50).Select(i => new Thread(() =>
        {
            start.Wait(); // release all 50 at the same moment
            _ = Naive.Instance; _ = Locked.Instance; _ = ViaLazy.Instance;
        })).ToList();
        threads.ForEach(t => t.Start());
        start.Set();
        threads.ForEach(t => t.Join());

        Console.WriteLine($"  naive constructor ran {naiveCreated} time(s)  ← more than 1 means the race happened");
        Check("Double-checked locking instances", lockedCreated, 1);
        Check("Lazy<T> instances", lazyCreated, 1);
    }
}
