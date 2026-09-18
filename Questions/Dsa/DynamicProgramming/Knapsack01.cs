namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_04, "0/1 Knapsack", Medium,
"Each item has a weight and a value and can be taken at most once. Maximize value without exceeding capacity W. Also report which items were chosen.")]
public static class Knapsack01
{
    // dp[i, w] = best value using the first i items with capacity w.
    // Either skip item i: dp[i-1, w], or take it: dp[i-1, w - weight] + value. Time O(n·W)
    public static (int Best, List<int> Items) Solve(int[] weight, int[] value, int W)
    {
        int n = weight.Length;
        var dp = new int[n + 1, W + 1];
        for (int i = 1; i <= n; i++)
            for (int w = 0; w <= W; w++)
            {
                dp[i, w] = dp[i - 1, w];
                if (weight[i - 1] <= w)
                    dp[i, w] = Math.Max(dp[i, w], dp[i - 1, w - weight[i - 1]] + value[i - 1]);
            }

        // Walk back: if the value changed when adding item i, it was taken.
        var chosen = new List<int>();
        for (int i = n, w = W; i > 0; i--)
            if (dp[i, w] != dp[i - 1, w]) { chosen.Add(i - 1); w -= weight[i - 1]; }
        chosen.Reverse();
        return (dp[n, W], chosen);
    }

    // Same answer with a 1D array: iterate w DOWNWARD so each item is used at most once.
    public static int SolveCompact(int[] weight, int[] value, int W)
    {
        var dp = new int[W + 1];
        for (int i = 0; i < weight.Length; i++)
            for (int w = W; w >= weight[i]; w--)
                dp[w] = Math.Max(dp[w], dp[w - weight[i]] + value[i]);
        return dp[W];
    }

    public static void Run()
    {
        var (best, items) = Solve([1, 3, 4, 5], [1, 4, 5, 7], 7);
        Check("weights [1,3,4,5], values [1,4,5,7], W=7", best, 9);
        Check("chosen item indices", items, [1, 2]);
        Check("SolveCompact same input", SolveCompact([1, 3, 4, 5], [1, 4, 5, 7], 7), 9);
        Check("weights [10,20,30], values [60,100,120], W=50", SolveCompact([10, 20, 30], [60, 100, 120], 50), 220);
    }
}
