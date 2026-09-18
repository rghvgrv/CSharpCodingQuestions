namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 4, Title = "Reverse the Words in a Sentence", Level = Medium, Problem = """
    Reverse the order of the words. Remove extra spaces so words are separated by exactly one space.
    `"  the sky  is blue "` → `"blue is sky the"`.
    """)]
public static class ReverseWords
{
    [Approach(Name = "Split, Reverse, Join", Time = "O(n)", Space = "O(n)", Idea = """
        Let the standard library do the work: split on spaces (dropping empty pieces), reverse the list of words, and join them with single spaces.
        """)]
    public static string ReverseWithSplit(string sentence)
    {
        string[] words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Array.Reverse(words);
        return string.Join(" ", words);
    }

    [Approach(Name = "Scan From the End", Time = "O(n)", Space = "O(n)", Idea = """
        Do it by hand, which is a common interview follow-up:

        1. Start at the end and skip spaces.
        2. Find where the word starts.
        3. Append that word to the result, then continue to the left.
        """)]
    public static string ReverseByScanning(string sentence)
    {
        var result = new StringBuilder();
        int i = sentence.Length - 1;

        while (i >= 0)
        {
            while (i >= 0 && sentence[i] == ' ')
            {
                i--;
            }
            if (i < 0)
            {
                break;
            }

            int wordEnd = i;
            while (i >= 0 && sentence[i] != ' ')
            {
                i--;
            }

            if (result.Length > 0)
            {
                result.Append(' ');
            }
            result.Append(sentence, i + 1, wordEnd - i);
        }
        return result.ToString();
    }

    public static Example[] Examples =>
    [
        new(["the sky is blue"], "blue is sky the"),
        new(["  hello world  "], "world hello"),
        new(["a good   example"], "example good a"),
    ];
}
