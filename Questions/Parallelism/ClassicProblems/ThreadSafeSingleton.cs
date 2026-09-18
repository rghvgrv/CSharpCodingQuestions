namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 7, Title = "Thread-Safe Singleton (Create Exactly Once)", Level = Medium, Problem = """
    An expensive object (like a configuration loader) must be created **exactly once**, even when 50 threads ask for it at the same moment.
    """)]
public static class ThreadSafeSingleton
{
    [Approach(Name = "Check for null (Broken)", Idea = """
        `if (instance == null) instance = new …` is a race: several threads can see `null` at the same time and each create one.
        """)]
    public class NaiveHolder
    {
        private ExpensiveThing? instance;

        public ExpensiveThing Get()
        {
            if (instance == null)
            {
                instance = new ExpensiveThing();
            }
            return instance;
        }
    }

    [Approach(Name = "lock on Every Call", Idea = """
        Put the check inside a `lock`. Correct, but **every** call takes the lock forever after, even though creation happened only once.
        """)]
    public class LockedHolder
    {
        private readonly object gate = new object();
        private ExpensiveThing? instance;

        public ExpensiveThing Get()
        {
            lock (gate)
            {
                if (instance == null)
                {
                    instance = new ExpensiveThing();
                }
                return instance;
            }
        }
    }

    [Approach(Name = "Double-Checked Locking", Idea = """
        Check **without** the lock first (fast, once it exists). Only if it's still `null`, take the lock and check **again**,
        because another thread may have created it while we waited for the lock.
        `volatile` stops other threads from seeing a half-built object. Easy to get subtly wrong, so prefer the next approach.
        """)]
    public class DoubleCheckedHolder
    {
        private readonly object gate = new object();
        private volatile ExpensiveThing? instance;

        public ExpensiveThing Get()
        {
            if (instance == null)
            {
                lock (gate)
                {
                    if (instance == null)
                    {
                        instance = new ExpensiveThing();
                    }
                }
            }
            return instance;
        }
    }

    [Approach(Name = "Lazy<T>", Idea = """
        `Lazy<T>` does all of the above for you, correctly: the first `.Value` creates the object (thread-safe by default)
        and every later call just returns it. This is the idiomatic C# answer.

        (A `static readonly` field is also created exactly once by the runtime, but it's created as soon as the class is first used, not on demand.)
        """)]
    public class LazyHolder
    {
        private readonly Lazy<ExpensiveThing> instance = new(() => new ExpensiveThing());

        public ExpensiveThing Get() => instance.Value;
    }

    public class ExpensiveThing
    {
        public static int Created;

        public ExpensiveThing()
        {
            Interlocked.Increment(ref Created);
            Thread.Sleep(20);   // pretend creating it is slow
        }
    }

    public static void Demo()
    {
        Print("Check for null: objects created", CountCreations(new NaiveHolder().Get) + " (more than 1 means the race happened)");
        Print("lock on every call: objects created", CountCreations(new LockedHolder().Get), expected: 1);
        Print("Double-checked locking: objects created", CountCreations(new DoubleCheckedHolder().Get), expected: 1);
        Print("Lazy<T>: objects created", CountCreations(new LazyHolder().Get), expected: 1);
    }

    private static readonly object measureGate = new object();

    private static int CountCreations(Func<ExpensiveThing> get)
    {
        lock (measureGate)   // one measurement at a time, since the counter is shared
        {
            ExpensiveThing.Created = 0;
            using var start = new ManualResetEventSlim();
            var threads = Enumerable.Range(0, 50).Select(_ => new Thread(() =>
            {
                start.Wait();
                get();
            })).ToList();
            threads.ForEach(thread => thread.Start());
            start.Set();
            threads.ForEach(thread => thread.Join());
            return ExpensiveThing.Created;
        }
    }
}
