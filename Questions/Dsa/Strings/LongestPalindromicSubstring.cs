namespace CodingQuestions.Dsa.Strings;

[Q(1_03_09, "Longest Palindromic Substring", Medium,
"Return the longest substring that is a palindrome.")]
public static class LongestPalindromicSubstring
{
    // Expand around each center. There are 2n-1 centers: each char (odd length) and each gap (even length).
    // Time O(n²), Space O(1)
    public static string Solve(string s)
    {
        int start = 0, length = 0;
        for (int center = 0; center < s.Length; center++)
        {
            foreach (var (l0, r0) in new[] { (center, center), (center, center + 1) })
            {
                int l = l0, r = r0;
                while (l >= 0 && r < s.Length && s[l] == s[r]) { l--; r++; }
                if (r - l - 1 > length) { start = l + 1; length = r - l - 1; }
            }
        }
        return s.Substring(start, length);
    }

    public static void Run()
    {
        Check("\"babad\"", Solve("babad"), "bab");
        Check("\"cbbd\"", Solve("cbbd"), "bb");
        Check("\"forgeeksskeegfor\"", Solve("forgeeksskeegfor"), "geeksskeeg");
        Check("\"a\"", Solve("a"), "a");
    }
}
