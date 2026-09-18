namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 2, Title = "First Non-Repeating Character", Level = Easy, Problem = """
    Return the position of the first character that appears only once, or `-1` if there is none.
    `"leetcode"` → `0` (`'l'`), `"loveleetcode"` → `2` (`'v'`).
    """)]
public static class FirstUniqueCharacter
{
    [Approach(Name = "Compare With Every Other Character", Time = "O(n²)", Space = "O(1)", Idea = """
        For each character, scan the whole string to see whether it appears anywhere else.
        """)]
    public static int FirstUniqueBruteForce(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            bool repeated = false;
            for (int j = 0; j < text.Length; j++)
            {
                if (i != j && text[i] == text[j])
                {
                    repeated = true;
                    break;
                }
            }
            if (!repeated)
            {
                return i;
            }
        }
        return -1;
    }

    [Approach(Name = "Count First, Then Find", Time = "O(n)", Space = "O(1)", Idea = """
        1. One pass to count how often each character appears (a dictionary).
        2. A second pass to find the first character whose count is 1.
        """)]
    public static int FirstUniqueByCounting(string text)
    {
        var countByLetter = new Dictionary<char, int>();
        foreach (char letter in text)
        {
            countByLetter[letter] = countByLetter.GetValueOrDefault(letter) + 1;
        }

        for (int i = 0; i < text.Length; i++)
        {
            if (countByLetter[text[i]] == 1)
            {
                return i;
            }
        }
        return -1;
    }

    public static Example[] Examples =>
    [
        new(["leetcode"], 0),
        new(["loveleetcode"], 2),
        new(["aabb"], -1),
    ];
}
