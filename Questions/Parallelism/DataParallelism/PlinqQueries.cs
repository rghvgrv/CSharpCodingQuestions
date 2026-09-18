namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 3, Title = "Parallel LINQ (PLINQ)", Level = Easy, Problem = """
    Find the first 10 prime numbers above 1,000,000 with a LINQ query, and make it run in parallel.
    """)]
public static class PlinqQueries
{
    private static bool IsPrime(int n)
    {
        for (int divisor = 2; divisor * divisor <= n; divisor++)
        {
            if (n % divisor == 0)
            {
                return false;
            }
        }
        return n > 1;
    }

    [Approach(Name = "Normal LINQ", Idea = """
        A LINQ query runs on one thread, checking the numbers one after another.
        """)]
    public static List<int> FirstPrimesLinq(int start, int count)
    {
        return Enumerable.Range(start, 200_000)
            .Where(IsPrime)
            .Take(count)
            .ToList();
    }

    [Approach(Name = "AsParallel (Order Not Kept)", Idea = """
        Add `.AsParallel()` and the rest of the query runs on all cores.
        But threads finish in any order, so **which** primes you get and their order can change from run to run. Fine for sums or counts, wrong here.
        """)]
    public static List<int> FirstPrimesUnordered(int start, int count)
    {
        return Enumerable.Range(start, 200_000)
            .AsParallel()
            .Where(IsPrime)
            .Take(count)
            .ToList();
    }

    [Approach(Name = "AsParallel().AsOrdered()", Idea = """
        `.AsOrdered()` tells PLINQ to keep the original order in the results. It still works in parallel,
        but buffers results so they come out in order. Use it only when order matters, because it costs some speed.

        `.WithDegreeOfParallelism(n)` limits how many cores the query may use.
        """)]
    public static List<int> FirstPrimesOrdered(int start, int count)
    {
        return Enumerable.Range(start, 200_000)
            .AsParallel()
            .AsOrdered()
            .Where(IsPrime)
            .Take(count)
            .ToList();
    }

    public static void Demo()
    {
        List<int> expected = FirstPrimesLinq(1_000_000, 10);
        Print("Normal LINQ", expected);
        Print("AsParallel (this run)", FirstPrimesUnordered(1_000_000, 10));
        Print("AsParallel().AsOrdered()", FirstPrimesOrdered(1_000_000, 10), expected: expected);

        int sequentialCount = Enumerable.Range(1, 2_000_000).Count(IsPrime);
        int parallelCount = Enumerable.Range(1, 2_000_000).AsParallel().Count(IsPrime);
        Print("Counting primes up to 2,000,000 (order doesn't matter)", parallelCount, expected: sequentialCount);
    }
}
