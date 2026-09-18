namespace CodingQuestions.Dsa.Strings;

[Q(1_03_04, "Reverse Words in a String", Medium,
"Reverse the order of words. Remove leading/trailing spaces and collapse multiple spaces to one.")]
public static class ReverseWords
{
    // One-liner with the standard library.
    public static string WithLibrary(string s) =>
        string.Join(' ', s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse());

    // By hand: scan from the end, copy each word.
    public static string ByHand(string s)
    {
        var sb = new StringBuilder();
        int i = s.Length - 1;
        while (i >= 0)
        {
            while (i >= 0 && s[i] == ' ') i--;
            if (i < 0) break;
            int end = i;
            while (i >= 0 && s[i] != ' ') i--;
            if (sb.Length > 0) sb.Append(' ');
            sb.Append(s, i + 1, end - i);
        }
        return sb.ToString();
    }

    public static void Run()
    {
        Check("WithLibrary(\"the sky is blue\")", WithLibrary("the sky is blue"), "blue is sky the");
        Check("ByHand(\"  hello world  \")", ByHand("  hello world  "), "world hello");
        Check("ByHand(\"a good   example\")", ByHand("a good   example"), "example good a");
    }
}
