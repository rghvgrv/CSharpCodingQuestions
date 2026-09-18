namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 9, Title = "Longest Palindromic Substring", Level = Medium, Problem = """
    Find the longest piece of the text that reads the same forwards and backwards.
    `"babad"` → `"bab"`. `"cbbd"` → `"bb"`.
    """)]
public static class LongestPalindromicSubstring
{
    [Approach(Name = "Check Every Substring", Time = "O(n³)", Space = "O(1)", Idea = """
        Try every start and end, and check whether that piece is a palindrome with two pointers.
        About n² substrings × n to check each one.
        """)]
    public static string LongestBruteForce(string text)
    {
        string best = "";
        for (int start = 0; start < text.Length; start++)
        {
            for (int end = start; end < text.Length; end++)
            {
                int length = end - start + 1;
                if (length > best.Length && IsPalindrome(text, start, end))
                {
                    best = text.Substring(start, length);
                }
            }
        }
        return best;
    }

    private static bool IsPalindrome(string text, int left, int right)
    {
        while (left < right)
        {
            if (text[left] != text[right])
            {
                return false;
            }
            left++;
            right--;
        }
        return true;
    }

    [Approach(Name = "Expand Around the Center", Time = "O(n²)", Space = "O(1)", Idea = """
        Every palindrome is mirrored around its center. So try each possible center and **grow outward** while the two sides match.

        A center can be one letter (odd length, like `"aba"`) or the gap between two letters (even length, like `"abba"`), so try both.
        """)]
    public static string LongestByExpanding(string text)
    {
        int bestStart = 0;
        int bestLength = 0;

        for (int center = 0; center < text.Length; center++)
        {
            int oddLength = ExpandLength(text, center, center);
            int evenLength = ExpandLength(text, center, center + 1);
            int length = Math.Max(oddLength, evenLength);

            if (length > bestLength)
            {
                bestLength = length;
                bestStart = center - (length - 1) / 2;
            }
        }
        return text.Substring(bestStart, bestLength);
    }

    private static int ExpandLength(string text, int left, int right)
    {
        while (left >= 0 && right < text.Length && text[left] == text[right])
        {
            left--;
            right++;
        }
        return right - left - 1;
    }

    public static Example[] Examples =>
    [
        new(["babad"], "bab"),
        new(["cbbd"], "bb"),
        new(["forgeeksskeegfor"], "geeksskeeg"),
    ];
}
