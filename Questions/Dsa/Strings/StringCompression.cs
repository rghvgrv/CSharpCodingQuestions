namespace CodingQuestions.Dsa.Strings;

[Q(1_03_06, "String Compression (Run-Length Encoding)", Easy,
"Compress \"aaabccdddd\" to \"a3bc2d4\" (count shown only when > 1). Then decode it back.")]
public static class StringCompression
{
    public static string Encode(string s)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < s.Length;)
        {
            int j = i;
            while (j < s.Length && s[j] == s[i]) j++;
            sb.Append(s[i]);
            if (j - i > 1) sb.Append(j - i);
            i = j;
        }
        return sb.ToString();
    }

    public static string Decode(string s)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < s.Length;)
        {
            char c = s[i++];
            int count = 0;
            while (i < s.Length && char.IsDigit(s[i])) count = count * 10 + (s[i++] - '0');
            sb.Append(c, Math.Max(count, 1));
        }
        return sb.ToString();
    }

    public static void Run()
    {
        Check("Encode(\"aaabccdddd\")", Encode("aaabccdddd"), "a3bc2d4");
        Check("Encode(\"abc\")", Encode("abc"), "abc");
        Check("Encode(\"zzzzzzzzzzzz\")", Encode("zzzzzzzzzzzz"), "z12");
        Check("Decode(\"a3bc2d4\")", Decode("a3bc2d4"), "aaabccdddd");
        Check("Decode(\"z12\")", Decode("z12"), "zzzzzzzzzzzz");
    }
}
