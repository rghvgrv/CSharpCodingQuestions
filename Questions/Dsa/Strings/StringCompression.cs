namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 6, Title = "String Compression", Level = Easy, Problem = """
    Replace each run of the same letter with the letter and the run length. Leave out the count when it's 1.
    `"aaabccdddd"` → `"a3bc2d4"`.
    """)]
public static class StringCompression
{
    [Approach(Name = "Build With +=", Time = "O(n²)", Space = "O(n)", Idea = """
        Count each run, then add the letter and count to the result with `+=`.
        Correct, but each `+=` copies the whole result so far, which is slow for long text.
        """)]
    public static string CompressWithConcatenation(string text)
    {
        string result = "";
        int i = 0;
        while (i < text.Length)
        {
            int runEnd = i;
            while (runEnd < text.Length && text[runEnd] == text[i])
            {
                runEnd++;
            }

            int runLength = runEnd - i;
            result += text[i];
            if (runLength > 1)
            {
                result += runLength;
            }
            i = runEnd;
        }
        return result;
    }

    [Approach(Name = "StringBuilder", Time = "O(n)", Space = "O(n)", Idea = """
        Same logic, but a `StringBuilder` appends in place without copying everything each time.
        Use it whenever you build a string in a loop.
        """)]
    public static string CompressWithBuilder(string text)
    {
        var result = new StringBuilder();
        int i = 0;
        while (i < text.Length)
        {
            int runEnd = i;
            while (runEnd < text.Length && text[runEnd] == text[i])
            {
                runEnd++;
            }

            int runLength = runEnd - i;
            result.Append(text[i]);
            if (runLength > 1)
            {
                result.Append(runLength);
            }
            i = runEnd;
        }
        return result.ToString();
    }

    public static Example[] Examples =>
    [
        new(["aaabccdddd"], "a3bc2d4"),
        new(["abc"], "abc"),
        new(["zzzzzzzzzzzz"], "z12"),
    ];
}
