namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 6, Title = "Longest Common Subsequence", Level = Medium, Problem = """
    Find the length of the longest sequence of letters that appears in **both** words in the same order (letters may be skipped).
    `"abcde"` and `"ace"` → `3` (`"ace"`).
    """)]
public static class LongestCommonSubsequence
{
    [Approach(Name = "Plain Recursion", Time = "O(2^(m + n))", Space = "O(m + n)", Idea = """
        Compare the first letters of both words:

        - **Same** → it's part of the answer: `1 + LCS(rest of first, rest of second)`.
        - **Different** → drop one letter from one word or the other, and keep the better result.
        """)]
    public static int LengthRecursive(string first, string second)
    {
        return Lcs(first, second, 0, 0);
    }

    private static int Lcs(string first, string second, int i, int j)
    {
        if (i == first.Length || j == second.Length)
        {
            return 0;
        }
        if (first[i] == second[j])
        {
            return 1 + Lcs(first, second, i + 1, j + 1);
        }
        return Math.Max(Lcs(first, second, i + 1, j), Lcs(first, second, i, j + 1));
    }

    [Approach(Name = "Table (Bottom-Up)", Time = "O(m · n)", Space = "O(m · n)", Idea = """
        `table[i, j]` = the LCS of the first `i` letters of `first` and the first `j` letters of `second`.

        - Letters equal → `table[i - 1, j - 1] + 1` (the diagonal cell + 1)
        - Different → `max(table[i - 1, j], table[i, j - 1])` (the cell above or to the left)

        Every cell is computed once. The answer is the bottom-right cell.
        """)]
    public static int LengthTable(string first, string second)
    {
        int[,] table = new int[first.Length + 1, second.Length + 1];
        for (int i = 1; i <= first.Length; i++)
        {
            for (int j = 1; j <= second.Length; j++)
            {
                if (first[i - 1] == second[j - 1])
                {
                    table[i, j] = table[i - 1, j - 1] + 1;
                }
                else
                {
                    table[i, j] = Math.Max(table[i - 1, j], table[i, j - 1]);
                }
            }
        }
        return table[first.Length, second.Length];
    }

    public static Example[] Examples =>
    [
        new(["abcde", "ace"], 3),
        new(["AGGTAB", "GXTXAYB"], 4),
        new(["abc", "def"], 0),
    ];
}
