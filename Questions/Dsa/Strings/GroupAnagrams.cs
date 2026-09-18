namespace CodingQuestions.Dsa.Strings;

[Q(1_03_08, "Group Anagrams", Medium,
"Group words that are anagrams of each other.")]
public static class GroupAnagrams
{
    // Key = the word's letters sorted. Anagrams share a key. Time O(n · k log k)
    public static List<List<string>> Solve(string[] words) =>
        words.GroupBy(w => new string(w.Order().ToArray()))
             .Select(g => g.ToList())
             .ToList();

    public static void Run()
    {
        Check("[eat, tea, tan, ate, nat, bat]",
            Solve(["eat", "tea", "tan", "ate", "nat", "bat"]),
            [["eat", "tea", "ate"], ["tan", "nat"], ["bat"]]);
        Check("[\"\"]", Solve([""]), [[""]]);
    }
}
