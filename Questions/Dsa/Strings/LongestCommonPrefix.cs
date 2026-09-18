namespace CodingQuestions.Dsa.Strings;

[Q(1_03_03, "Longest Common Prefix", Easy,
"Find the longest prefix shared by every string in the array.")]
public static class LongestCommonPrefix
{
    // Vertical scan: compare column by column until a string ends or a character differs.
    public static string Solve(string[] words)
    {
        if (words.Length == 0) return "";
        for (int i = 0; i < words[0].Length; i++)
            foreach (var w in words)
                if (i == w.Length || w[i] != words[0][i]) return words[0][..i];
        return words[0];
    }

    public static void Run()
    {
        Check("[flower, flow, flight]", Solve(["flower", "flow", "flight"]), "fl");
        Check("[dog, racecar, car]", Solve(["dog", "racecar", "car"]), "");
        Check("[interview, internet, interval]", Solve(["interview", "internet", "interval"]), "inter");
    }
}
