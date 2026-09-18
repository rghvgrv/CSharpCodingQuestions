namespace CodingQuestions.Dsa.Strings;

[Q(1_03_01, "Valid Anagram", Easy,
"Return true if t is an anagram of s (same letters, same counts, any order). Lowercase English letters only.")]
public static class ValidAnagram
{
    // Count letters up for s, down for t. Every count must end at 0. Time O(n), Space O(26)
    public static bool Solve(string s, string t)
    {
        if (s.Length != t.Length) return false;
        var count = new int[26];
        for (int i = 0; i < s.Length; i++)
        {
            count[s[i] - 'a']++;
            count[t[i] - 'a']--;
        }
        return count.All(c => c == 0);
    }

    public static void Run()
    {
        Check("(\"anagram\", \"nagaram\")", Solve("anagram", "nagaram"), true);
        Check("(\"rat\", \"car\")", Solve("rat", "car"), false);
        Check("(\"listen\", \"silent\")", Solve("listen", "silent"), true);
    }
}
