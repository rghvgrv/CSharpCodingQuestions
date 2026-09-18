namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 10, Title = "Minimum Window Substring", Level = Hard, Problem = """
    Find the shortest piece of `text` that contains every character of `required` (repeated characters must be there as many times).
    Return `""` if there's none.
    `text = "ADOBECODEBANC"`, `required = "ABC"` → `"BANC"`.
    """)]
public static class MinimumWindowSubstring
{
    [Approach(Name = "Try Every Start", Time = "O(n²)", Space = "O(k)", Idea = """
        From each start position, extend to the right until the piece contains everything required, and keep the shortest one.
        A counter `missing` tracks how many required characters are still not covered.
        """)]
    public static string MinWindowBruteForce(string text, string required)
    {
        string best = "";
        for (int start = 0; start < text.Length; start++)
        {
            var needed = CountLetters(required);
            int missing = required.Length;

            for (int end = start; end < text.Length; end++)
            {
                char letter = text[end];
                if (needed.ContainsKey(letter))
                {
                    if (needed[letter] > 0)
                    {
                        missing--;
                    }
                    needed[letter]--;
                }

                if (missing == 0)
                {
                    int length = end - start + 1;
                    if (best == "" || length < best.Length)
                    {
                        best = text.Substring(start, length);
                    }
                    break;
                }
            }
        }
        return best;
    }

    private static Dictionary<char, int> CountLetters(string text)
    {
        var counts = new Dictionary<char, int>();
        foreach (char letter in text)
        {
            counts[letter] = counts.GetValueOrDefault(letter) + 1;
        }
        return counts;
    }

    [Approach(Name = "Sliding Window", Time = "O(n + k)", Space = "O(k)", Idea = """
        Use one window `[left, right]` that moves only forward:

        1. **Grow**: move `right` and count what the window now covers.
        2. When everything is covered, **shrink**: move `left` forward while the window still covers everything, recording the shortest window.
        3. As soon as removing a character breaks the coverage, go back to growing.

        Each character is added once and removed once, so this is linear.
        """)]
    public static string MinWindowSliding(string text, string required)
    {
        var needed = new Dictionary<char, int>();
        foreach (char letter in required)
        {
            needed[letter] = needed.GetValueOrDefault(letter) + 1;
        }

        int missing = required.Length;
        int left = 0;
        int bestStart = 0;
        int bestLength = int.MaxValue;

        for (int right = 0; right < text.Length; right++)
        {
            char added = text[right];
            if (needed.ContainsKey(added))
            {
                if (needed[added] > 0)
                {
                    missing--;
                }
                needed[added]--;
            }

            while (missing == 0)
            {
                if (right - left + 1 < bestLength)
                {
                    bestStart = left;
                    bestLength = right - left + 1;
                }

                char removed = text[left];
                if (needed.ContainsKey(removed))
                {
                    needed[removed]++;
                    if (needed[removed] > 0)
                    {
                        missing++;
                    }
                }
                left++;
            }
        }
        return bestLength == int.MaxValue ? "" : text.Substring(bestStart, bestLength);
    }

    public static Example[] Examples =>
    [
        new(["ADOBECODEBANC", "ABC"], "BANC"),
        new(["a", "a"], "a"),
        new(["a", "aa"], ""),
    ];
}
