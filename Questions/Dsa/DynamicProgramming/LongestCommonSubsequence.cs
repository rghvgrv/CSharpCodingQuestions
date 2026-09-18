namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_06, "Longest Common Subsequence", Medium,
"Find the length of the longest subsequence common to two strings, and print one such subsequence.")]
public static class LongestCommonSubsequence
{
    // dp[i, j] = LCS of a[..i] and b[..j].
    // Equal last chars: 1 + dp[i-1, j-1]. Otherwise: max(dp[i-1, j], dp[i, j-1]). Time O(m·n)
    public static (int Length, string Lcs) Solve(string a, string b)
    {
        int m = a.Length, n = b.Length;
        var dp = new int[m + 1, n + 1];
        for (int i = 1; i <= m; i++)
            for (int j = 1; j <= n; j++)
                dp[i, j] = a[i - 1] == b[j - 1] ? dp[i - 1, j - 1] + 1 : Math.Max(dp[i - 1, j], dp[i, j - 1]);

        // Rebuild the subsequence by walking back from dp[m, n].
        var sb = new StringBuilder();
        for (int i = m, j = n; i > 0 && j > 0;)
        {
            if (a[i - 1] == b[j - 1]) { sb.Insert(0, a[i - 1]); i--; j--; }
            else if (dp[i - 1, j] >= dp[i, j - 1]) i--;
            else j--;
        }
        return (dp[m, n], sb.ToString());
    }

    public static void Run()
    {
        Check("(\"abcde\", \"ace\")", Solve("abcde", "ace"), (3, "ace"));
        Check("(\"AGGTAB\", \"GXTXAYB\")", Solve("AGGTAB", "GXTXAYB"), (4, "GTAB"));
        Check("(\"abc\", \"def\")", Solve("abc", "def"), (0, ""));
    }
}
