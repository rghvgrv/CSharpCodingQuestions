namespace CodingQuestions.Dsa.Strings;

[Q(1_03_11, "Pattern Search (KMP)", Hard,
"Find every index where pattern occurs in text in O(n + m) using the Knuth–Morris–Pratt algorithm.")]
public static class KmpPatternSearch
{
    // lps[i] = length of the longest proper prefix of pattern[0..i] that is also a suffix.
    // After a mismatch we fall back via lps instead of restarting, so text is never re-scanned.
    public static int[] BuildLps(string p)
    {
        var lps = new int[p.Length];
        for (int i = 1, len = 0; i < p.Length;)
        {
            if (p[i] == p[len]) lps[i++] = ++len;
            else if (len > 0) len = lps[len - 1];
            else lps[i++] = 0;
        }
        return lps;
    }

    public static List<int> Search(string text, string p)
    {
        var lps = BuildLps(p);
        var found = new List<int>();
        for (int i = 0, j = 0; i < text.Length;)
        {
            if (text[i] == p[j])
            {
                i++; j++;
                if (j == p.Length) { found.Add(i - j); j = lps[j - 1]; }
            }
            else if (j > 0) j = lps[j - 1];
            else i++;
        }
        return found;
    }

    public static void Run()
    {
        Check("BuildLps(\"ABABCABAB\")", BuildLps("ABABCABAB"), [0, 0, 1, 2, 0, 1, 2, 3, 4]);
        Check("Search(\"ABABDABACDABABCABAB\", \"ABABCABAB\")", Search("ABABDABACDABABCABAB", "ABABCABAB"), [10]);
        Check("Search(\"AAAAA\", \"AA\")", Search("AAAAA", "AA"), [0, 1, 2, 3]);
    }
}
