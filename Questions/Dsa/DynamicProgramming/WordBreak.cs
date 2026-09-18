namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 9, Title = "Word Break", Level = Medium, Problem = """
    Can `text` be split into a sequence of words from the dictionary? Words may be reused.
    `"applepenapple"` with `["apple", "pen"]` → `true` ("apple pen apple").
    """)]
public static class WordBreak
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        Try every dictionary word that the text **starts with**, and recursively check whether the rest can be split.
        The same leftover text gets checked many times.
        """)]
    public static bool CanBreakRecursive(string text, string[] words)
    {
        if (text.Length == 0)
        {
            return true;
        }
        foreach (string word in words)
        {
            if (text.StartsWith(word) && CanBreakRecursive(text.Substring(word.Length), words))
            {
                return true;
            }
        }
        return false;
    }

    [Approach(Name = "Table of Breakable Prefixes", Time = "O(n²)", Space = "O(n)", Idea = """
        `canBreak[i]` = "the first `i` letters can be split into words". `canBreak[0]` is `true` (the empty start).

        `canBreak[i]` is true if there's some earlier `j` with `canBreak[j]` true **and** the letters from `j` to `i` form a dictionary word.
        A `HashSet` makes the word check instant.
        """)]
    public static bool CanBreakTable(string text, string[] words)
    {
        var dictionary = new HashSet<string>(words);
        bool[] canBreak = new bool[text.Length + 1];
        canBreak[0] = true;

        for (int end = 1; end <= text.Length; end++)
        {
            for (int start = 0; start < end; start++)
            {
                if (canBreak[start] && dictionary.Contains(text.Substring(start, end - start)))
                {
                    canBreak[end] = true;
                    break;
                }
            }
        }
        return canBreak[text.Length];
    }

    public static Example[] Examples =>
    [
        new(["leetcode", new[] { "leet", "code" }], true),
        new(["applepenapple", new[] { "apple", "pen" }], true),
        new(["catsandog", new[] { "cats", "dog", "sand", "and", "cat" }], false),
    ];
}
