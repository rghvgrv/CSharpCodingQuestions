namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 11, Title = "Find a Pattern in Text (KMP)", Level = Hard, Problem = """
    Return every position where `pattern` appears in `text`.
    `text = "AABAACAADAABAABA"`, `pattern = "AABA"` → `[0, 9, 12]`.
    """)]
public static class PatternSearch
{
    [Approach(Name = "Check Every Position", Time = "O(n · m)", Space = "O(1) extra", Idea = """
        Line the pattern up at every position of the text and compare letter by letter. `n` = text length, `m` = pattern length.
        On texts like `"AAAAAAAB"` it re-checks the same letters many times.
        """)]
    public static List<int> SearchNaive(string text, string pattern)
    {
        var positions = new List<int>();
        for (int start = 0; start + pattern.Length <= text.Length; start++)
        {
            int matched = 0;
            while (matched < pattern.Length && text[start + matched] == pattern[matched])
            {
                matched++;
            }
            if (matched == pattern.Length)
            {
                positions.Add(start);
            }
        }
        return positions;
    }

    [Approach(Name = "Knuth–Morris–Pratt (KMP)", Time = "O(n + m)", Space = "O(m)", Idea = """
        When a mismatch happens after matching some letters, the naive way starts over. KMP **never moves backwards in the text**.

        1. Precompute `longest[i]`: the length of the longest proper beginning of `pattern[0..i]` that is also its ending.
           For `"AABA"` that's `[0, 1, 0, 1]`.
        2. While scanning the text, after a mismatch jump the pattern position back to `longest[matched - 1]`,
           because those letters are already known to match.
        """)]
    public static List<int> SearchKmp(string text, string pattern)
    {
        int[] longest = BuildLongestPrefixSuffix(pattern);
        var positions = new List<int>();
        int matched = 0;

        for (int i = 0; i < text.Length; i++)
        {
            while (matched > 0 && text[i] != pattern[matched])
            {
                matched = longest[matched - 1];
            }
            if (text[i] == pattern[matched])
            {
                matched++;
            }
            if (matched == pattern.Length)
            {
                positions.Add(i - pattern.Length + 1);
                matched = longest[matched - 1];
            }
        }
        return positions;
    }

    private static int[] BuildLongestPrefixSuffix(string pattern)
    {
        int[] longest = new int[pattern.Length];
        int length = 0;
        for (int i = 1; i < pattern.Length; i++)
        {
            while (length > 0 && pattern[i] != pattern[length])
            {
                length = longest[length - 1];
            }
            if (pattern[i] == pattern[length])
            {
                length++;
            }
            longest[i] = length;
        }
        return longest;
    }

    public static Example[] Examples =>
    [
        new(["AABAACAADAABAABA", "AABA"], new[] { 0, 9, 12 }),
        new(["AAAAA", "AA"], new[] { 0, 1, 2, 3 }),
        new(["hello", "xyz"], Array.Empty<int>()),
    ];
}
