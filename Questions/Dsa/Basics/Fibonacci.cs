namespace CodingQuestions.Dsa.Basics;

[Q(1_01_05, "Fibonacci", Easy,
"Return the nth Fibonacci number (F0 = 0, F1 = 1). Compare plain recursion, memoization and the iterative version.")]
public static class Fibonacci
{
    // Time O(2^n): recomputes the same values again and again.
    public static long Naive(int n) => n < 2 ? n : Naive(n - 1) + Naive(n - 2);

    // Time O(n), Space O(n): cache every answer (top-down DP).
    public static long Memo(int n, Dictionary<int, long>? memo = null)
    {
        memo ??= [];
        if (n < 2) return n;
        if (memo.TryGetValue(n, out var cached)) return cached;
        return memo[n] = Memo(n - 1, memo) + Memo(n - 2, memo);
    }

    // Time O(n), Space O(1): keep only the last two values (bottom-up DP).
    public static long Iterative(int n)
    {
        long a = 0, b = 1;
        for (int i = 0; i < n; i++) (a, b) = (b, a + b);
        return a;
    }

    public static void Run()
    {
        Check("First 10", Enumerable.Range(0, 10).Select(Iterative), [0L, 1, 1, 2, 3, 5, 8, 13, 21, 34]);

        var sw = Stopwatch.StartNew();
        Check("Naive(30)", Naive(30), 832040L);
        Console.WriteLine($"  naive took {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        Check("Memo(90)", Memo(90), 2880067194370816120L);
        Check("Iterative(90)", Iterative(90), 2880067194370816120L);
        Console.WriteLine($"  memo + iterative took {sw.Elapsed.TotalMilliseconds:0.###} ms");
    }
}
