namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 4, Title = "Run Different Jobs at Once (Parallel.Invoke)", Level = Easy, Problem = """
    Three **different** CPU-heavy jobs have to run: count primes, sum some squares, and compute a big factorial's digit count.
    They don't depend on each other. Run them at the same time.
    """)]
public static class RunDifferentJobs
{
    private static int CountPrimes(int limit)
    {
        int count = 0;
        for (int n = 2; n < limit; n++)
        {
            bool prime = true;
            for (int d = 2; d * d <= n; d++)
            {
                if (n % d == 0)
                {
                    prime = false;
                    break;
                }
            }
            if (prime)
            {
                count++;
            }
        }
        return count;
    }

    private static long SumOfSquares(int limit)
    {
        long sum = 0;
        for (long i = 1; i <= limit; i++)
        {
            sum += i * i % 1000;
        }
        return sum;
    }

    private static int FactorialDigits(int n)
    {
        System.Numerics.BigInteger product = 1;
        for (int i = 2; i <= n; i++)
        {
            product *= i;
        }
        return product.ToString().Length;
    }

    [Approach(Name = "One After Another", Idea = """
        Run the three jobs in sequence. The total time is the **sum** of the three times.
        """)]
    public static (int Primes, long Squares, int Digits) RunSequentially()
    {
        int primes = CountPrimes(300_000);
        long squares = SumOfSquares(30_000_000);
        int digits = FactorialDigits(5_000);
        return (primes, squares, digits);
    }

    [Approach(Name = "Parallel.Invoke", Idea = """
        `Parallel.Invoke` takes several actions, runs them at the same time, and returns when **all** are done.
        The total time becomes roughly the time of the **slowest** job.
        Each job writes its own variable, so nothing is shared.
        """)]
    public static (int Primes, long Squares, int Digits) RunTogether()
    {
        int primes = 0;
        long squares = 0;
        int digits = 0;
        Parallel.Invoke(
            () => primes = CountPrimes(300_000),
            () => squares = SumOfSquares(30_000_000),
            () => digits = FactorialDigits(5_000));
        return (primes, squares, digits);
    }

    public static void Demo()
    {
        var clock = Stopwatch.StartNew();
        var sequential = RunSequentially();
        long sequentialMs = clock.ElapsedMilliseconds;

        clock.Restart();
        var together = RunTogether();
        long togetherMs = clock.ElapsedMilliseconds;

        Print("One after another", $"{sequential} in {sequentialMs} ms");
        Print("Parallel.Invoke", $"{together} in {togetherMs} ms");
        Print("Same results", together, expected: sequential);
    }
}
