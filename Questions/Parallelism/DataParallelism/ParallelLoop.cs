namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 1, Title = "Use Every Core (Parallel.For)", Level = Easy, Problem = """
    For each number from 0 to 199,999, check whether it's prime, and store the answer in an array.
    Every check is independent of the others. Make it use all CPU cores.
    """)]
public static class ParallelLoop
{
    private static bool IsPrime(int n)
    {
        if (n < 2)
        {
            return false;
        }
        for (int divisor = 2; (long)divisor * divisor <= n; divisor++)
        {
            if (n % divisor == 0)
            {
                return false;
            }
        }
        return true;
    }

    [Approach(Name = "Normal for Loop", Idea = """
        One thread does all the work while the other cores sit idle.
        """)]
    public static bool[] CheckSequential(int count)
    {
        bool[] isPrime = new bool[count];
        for (int i = 0; i < count; i++)
        {
            isPrime[i] = IsPrime(i);
        }
        return isPrime;
    }

    [Approach(Name = "Parallel.For", Idea = """
        `Parallel.For` splits the range into chunks and runs them on several thread-pool threads at once. It returns when all are done.

        Each index writes only **its own** slot `isPrime[i]`, so no lock is needed.
        Parallel loops pay off only for **CPU-heavy** and **independent** iterations; for tiny work, the overhead can make them slower.
        """)]
    public static bool[] CheckParallel(int count)
    {
        bool[] isPrime = new bool[count];
        Parallel.For(0, count, i =>
        {
            isPrime[i] = IsPrime(i);
        });
        return isPrime;
    }

    public static void Demo()
    {
        const int count = 200_000;
        var clock = Stopwatch.StartNew();
        bool[] sequential = CheckSequential(count);
        long sequentialMs = clock.ElapsedMilliseconds;

        clock.Restart();
        bool[] parallel = CheckParallel(count);
        long parallelMs = clock.ElapsedMilliseconds;

        Print("Normal loop", $"{sequentialMs} ms");
        Print("Parallel.For", $"{parallelMs} ms on {Environment.ProcessorCount} cores");
        Print("Primes found", parallel.Count(isPrime => isPrime), expected: 17984);
        Print("Same answers", sequential.SequenceEqual(parallel), expected: true);
    }
}
