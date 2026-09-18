namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_07, "Edit Distance (Levenshtein)", Hard,
"Find the minimum number of inserts, deletes and replaces needed to turn word1 into word2.")]
public static class EditDistance
{
    // dp[i, j] = distance between a[..i] and b[..j].
    // Same char: dp[i-1, j-1]. Else 1 + min(replace dp[i-1, j-1], delete dp[i-1, j], insert dp[i, j-1]).
    public static int Solve(string a, string b)
    {
        int m = a.Length, n = b.Length;
        var dp = new int[m + 1, n + 1];
        for (int i = 0; i <= m; i++) dp[i, 0] = i; // delete everything
        for (int j = 0; j <= n; j++) dp[0, j] = j; // insert everything
        for (int i = 1; i <= m; i++)
            for (int j = 1; j <= n; j++)
                dp[i, j] = a[i - 1] == b[j - 1]
                    ? dp[i - 1, j - 1]
                    : 1 + Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1]));
        return dp[m, n];
    }

    public static void Run()
    {
        Check("(\"horse\", \"ros\")", Solve("horse", "ros"), 3);
        Check("(\"intention\", \"execution\")", Solve("intention", "execution"), 5);
        Check("(\"kitten\", \"sitting\")", Solve("kitten", "sitting"), 3);
        Check("(\"\", \"abc\")", Solve("", "abc"), 3);
    }
}
