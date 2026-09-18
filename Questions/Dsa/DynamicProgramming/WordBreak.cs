namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_09, "Word Break", Medium,
"Can string s be split into a sequence of dictionary words? Words may be reused.")]
public static class WordBreak
{
    // ok[i] = s[..i] can be segmented. ok[i] is true if some ok[j] is true and s[j..i] is a word.
    public static bool Solve(string s, string[] words)
    {
        var dict = new HashSet<string>(words);
        int maxLen = words.Max(w => w.Length);
        var ok = new bool[s.Length + 1];
        ok[0] = true;
        for (int i = 1; i <= s.Length; i++)
            for (int j = Math.Max(0, i - maxLen); j < i && !ok[i]; j++)
                ok[i] = ok[j] && dict.Contains(s[j..i]);
        return ok[s.Length];
    }

    public static void Run()
    {
        Check("\"leetcode\", [leet, code]", Solve("leetcode", ["leet", "code"]), true);
        Check("\"applepenapple\", [apple, pen]", Solve("applepenapple", ["apple", "pen"]), true);
        Check("\"catsandog\", [cats, dog, sand, and, cat]", Solve("catsandog", ["cats", "dog", "sand", "and", "cat"]), false);
    }
}
