namespace CodingQuestions.Dsa.Strings;

[Q(1_03_10, "Minimum Window Substring", Hard,
"Find the smallest substring of s that contains every character of t (including duplicates). Return \"\" if none.")]
public static class MinimumWindowSubstring
{
    // Grow the window right until it covers t, then shrink from the left while it still covers t.
    // `missing` = how many characters of t the window still lacks. Time O(|s| + |t|)
    public static string Solve(string s, string t)
    {
        var need = new Dictionary<char, int>();
        foreach (char c in t) need[c] = need.GetValueOrDefault(c) + 1;

        int missing = t.Length, left = 0, bestStart = 0, bestLen = int.MaxValue;
        for (int right = 0; right < s.Length; right++)
        {
            if (need.TryGetValue(s[right], out int n))
            {
                if (n > 0) missing--;
                need[s[right]] = n - 1;
            }

            while (missing == 0)
            {
                if (right - left + 1 < bestLen) { bestStart = left; bestLen = right - left + 1; }
                if (need.TryGetValue(s[left], out int m))
                {
                    need[s[left]] = m + 1;
                    if (m + 1 > 0) missing++;
                }
                left++;
            }
        }
        return bestLen == int.MaxValue ? "" : s.Substring(bestStart, bestLen);
    }

    public static void Run()
    {
        Check("(\"ADOBECODEBANC\", \"ABC\")", Solve("ADOBECODEBANC", "ABC"), "BANC");
        Check("(\"a\", \"a\")", Solve("a", "a"), "a");
        Check("(\"a\", \"aa\")", Solve("a", "aa"), "");
    }
}
