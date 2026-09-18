namespace CodingQuestions.Dsa.Strings;

[Q(1_03_02, "First Non-Repeating Character", Easy,
"Return the index of the first character that appears only once in the string, or -1.")]
public static class FirstUniqueCharacter
{
    // Two passes: count everything, then find the first count of 1. Time O(n)
    public static int Solve(string s)
    {
        var count = new Dictionary<char, int>();
        foreach (char c in s) count[c] = count.GetValueOrDefault(c) + 1;
        for (int i = 0; i < s.Length; i++)
            if (count[s[i]] == 1) return i;
        return -1;
    }

    public static void Run()
    {
        Check("\"leetcode\"", Solve("leetcode"), 0);
        Check("\"loveleetcode\"", Solve("loveleetcode"), 2);
        Check("\"aabb\"", Solve("aabb"), -1);
    }
}
