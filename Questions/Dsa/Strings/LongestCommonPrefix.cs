namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 3, Title = "Longest Common Prefix", Level = Easy, Problem = """
    Find the longest beginning that all the words share.
    `["flower", "flow", "flight"]` → `"fl"`. No shared beginning → `""`.
    """)]
public static class LongestCommonPrefix
{
    [Approach(Name = "Shrink the Prefix", Time = "O(S)", Space = "O(1)", Idea = """
        Start with the whole first word as the prefix. For each other word, cut letters off the end of the prefix
        until the word starts with it. `S` is the total number of letters in all words.
        """)]
    public static string PrefixByShrinking(string[] words)
    {
        string prefix = words[0];
        foreach (string word in words)
        {
            while (!word.StartsWith(prefix))
            {
                prefix = prefix.Substring(0, prefix.Length - 1);
            }
        }
        return prefix;
    }

    [Approach(Name = "Column by Column", Time = "O(S)", Space = "O(1)", Idea = """
        Compare the words letter by letter, like reading down a column: first letters of all words, then second letters, and so on.
        Stop at the first column where a word ends or a letter differs. This never looks past the answer.
        """)]
    public static string PrefixByColumns(string[] words)
    {
        for (int i = 0; i < words[0].Length; i++)
        {
            char letter = words[0][i];
            foreach (string word in words)
            {
                if (i == word.Length || word[i] != letter)
                {
                    return words[0].Substring(0, i);
                }
            }
        }
        return words[0];
    }

    public static Example[] Examples =>
    [
        new([new[] { "flower", "flow", "flight" }], "fl"),
        new([new[] { "interview", "internet", "interval" }], "inter"),
        new([new[] { "dog", "racecar", "car" }], ""),
    ];
}
