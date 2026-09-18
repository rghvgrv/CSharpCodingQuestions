namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 8, Title = "Group Anagrams", Level = Medium, Problem = """
    Put words that are anagrams of each other (same letters, different order) into the same group.
    `["eat", "tea", "tan", "ate", "nat", "bat"]` → `[["eat", "tea", "ate"], ["tan", "nat"], ["bat"]]`.
    """)]
public static class GroupAnagrams
{
    [Approach(Name = "Sorted Letters as the Key", Time = "O(n · k log k)", Space = "O(n · k)", Idea = """
        Anagrams have the same letters, so sorting their letters gives the same text: `"eat"`, `"tea"` and `"ate"` all become `"aet"`.
        Use that sorted text as a dictionary key, and add each word to its key's group.
        `n` = number of words, `k` = length of a word.
        """)]
    public static List<List<string>> GroupBySortedKey(string[] words)
    {
        var groups = new Dictionary<string, List<string>>();
        foreach (string word in words)
        {
            char[] letters = word.ToCharArray();
            Array.Sort(letters);
            string key = new string(letters);

            if (!groups.ContainsKey(key))
            {
                groups[key] = new List<string>();
            }
            groups[key].Add(word);
        }
        return groups.Values.ToList();
    }

    [Approach(Name = "Letter Counts as the Key", Time = "O(n · k)", Space = "O(n · k)", Idea = """
        Skip the sorting: count the 26 letters of each word and turn the counts into a key like `"1,0,0,0,1,…"`.
        Anagrams have identical counts, so they get the same key.
        """)]
    public static List<List<string>> GroupByCountKey(string[] words)
    {
        var groups = new Dictionary<string, List<string>>();
        foreach (string word in words)
        {
            int[] counts = new int[26];
            foreach (char letter in word)
            {
                counts[letter - 'a']++;
            }
            string key = string.Join(",", counts);

            if (!groups.ContainsKey(key))
            {
                groups[key] = new List<string>();
            }
            groups[key].Add(word);
        }
        return groups.Values.ToList();
    }

    public static Example[] Examples =>
    [
        new([new[] { "eat", "tea", "tan", "ate", "nat", "bat" }], new[] { new[] { "eat", "tea", "ate" }, new[] { "tan", "nat" }, new[] { "bat" } }),
        new([new[] { "abc" }], new[] { new[] { "abc" } }),
    ];
}
