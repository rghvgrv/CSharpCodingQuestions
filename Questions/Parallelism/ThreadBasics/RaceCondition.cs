namespace CSharpCodingQuestions.Questions.Parallelism.ThreadBasics;

[Question(Order = 3, Title = "Race Condition: Lost Updates", Level = Easy, Problem = """
    4 threads each add 1 to a shared counter 100,000 times. The answer should be 400,000. Is it?
    """)]
public static class RaceCondition
{
    [Approach(Name = "No Protection (Broken)", Idea = """
        `counter++` looks like one step but is really three: **read** the value, **add** 1, **write** it back.
        Two threads can read the same value (say 41), both write 42, and one of the increments is lost.
        The result is wrong, and different every run.
        """)]
    public static int CountUnsafe(int threadCount, int incrementsPerThread)
    {
        int counter = 0;
        var threads = new List<Thread>();
        for (int t = 0; t < threadCount; t++)
        {
            threads.Add(new Thread(() =>
            {
                for (int i = 0; i < incrementsPerThread; i++)
                {
                    counter++;
                }
            }));
        }
        threads.ForEach(thread => thread.Start());
        threads.ForEach(thread => thread.Join());
        return counter;
    }

    [Approach(Name = "lock", Idea = """
        A `lock` lets only **one** thread at a time run the code inside it.
        The read-add-write now happens without interruption. Correct, but the threads have to take turns.
        """)]
    public static int CountWithLock(int threadCount, int incrementsPerThread)
    {
        int counter = 0;
        object gate = new object();
        var threads = new List<Thread>();
        for (int t = 0; t < threadCount; t++)
        {
            threads.Add(new Thread(() =>
            {
                for (int i = 0; i < incrementsPerThread; i++)
                {
                    lock (gate)
                    {
                        counter++;
                    }
                }
            }));
        }
        threads.ForEach(thread => thread.Start());
        threads.ForEach(thread => thread.Join());
        return counter;
    }

    [Approach(Name = "Interlocked.Increment", Idea = """
        `Interlocked.Increment` asks the CPU to do the whole read-add-write as **one indivisible (atomic) step**.
        No lock is needed, and it's faster. This is the best tool for simple counters.
        """)]
    public static int CountWithInterlocked(int threadCount, int incrementsPerThread)
    {
        int counter = 0;
        var threads = new List<Thread>();
        for (int t = 0; t < threadCount; t++)
        {
            threads.Add(new Thread(() =>
            {
                for (int i = 0; i < incrementsPerThread; i++)
                {
                    Interlocked.Increment(ref counter);
                }
            }));
        }
        threads.ForEach(thread => thread.Start());
        threads.ForEach(thread => thread.Join());
        return counter;
    }

    public static void Demo()
    {
        int unsafeResult = CountUnsafe(4, 100_000);
        Print("No protection", $"{unsafeResult:N0} ({400_000 - unsafeResult:N0} updates lost)");
        Print("lock", CountWithLock(4, 100_000), expected: 400_000);
        Print("Interlocked", CountWithInterlocked(4, 100_000), expected: 400_000);
    }
}
