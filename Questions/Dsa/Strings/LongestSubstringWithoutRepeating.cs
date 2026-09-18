namespace CodingQuestions.Dsa.Strings;

[Q(1_03_07, "Longest Substring Without Repeating Characters", Medium,
"Find the length of the longest substring with no repeated characters. This is the classic sliding window problem.")]
public static class LongestSubstringWithoutRepeating
{
    // Sliding window [left, i]. When s[i] was seen inside the window, jump left past its last position.
    // Time O(n), Space O(charset)
    public static int Solve(string s)
    {
        var last = new Dictionary<char, int>();
        int left = 0, best = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (last.TryGetValue(s[i], out int prev) && prev >= left) left = prev + 1;
            last[s[i]] = i;
            best = Math.Max(best, i - left + 1);
        }
        return best;
    }

    public static void Run()
    {
        Check("\"abcabcbb\"", Solve("abcabcbb"), 3);
        Check("\"bbbbb\"", Solve("bbbbb"), 1);
        Check("\"pwwkew\"", Solve("pwwkew"), 3);
        Check("\"abba\"", Solve("abba"), 2);
        Check("\"\"", Solve(""), 0);
    }
}
