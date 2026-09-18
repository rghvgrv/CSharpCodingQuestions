namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 7, Title = "Longest Substring Without Repeating Characters", Level = Medium, Problem = """
    Find the length of the longest piece of the text that has no repeated character.
    `"abcabcbb"` → `3` (`"abc"`). `"pwwkew"` → `3` (`"wke"`).
    """)]
public static class LongestSubstringWithoutRepeating
{
    [Approach(Name = "Try Every Start", Time = "O(n²)", Space = "O(k)", Idea = """
        For each starting position, extend to the right, adding characters to a set, until a character repeats.
        Keep the longest length found. `k` is the number of different characters.
        """)]
    public static int LongestBruteForce(string text)
    {
        int best = 0;
        for (int start = 0; start < text.Length; start++)
        {
            var seen = new HashSet<char>();
            int end = start;
            while (end < text.Length && seen.Add(text[end]))
            {
                end++;
            }
            best = Math.Max(best, end - start);
        }
        return best;
    }

    [Approach(Name = "Sliding Window With a Set", Time = "O(n)", Space = "O(k)", Idea = """
        Keep a **window** `[left, right]` with no repeats, and the set of its characters.

        - Move `right` forward one character at a time.
        - If that character is already in the window, remove characters from the `left` until it isn't.

        Each character enters and leaves the window at most once, so this is linear.
        """)]
    public static int LongestSlidingWindow(string text)
    {
        var window = new HashSet<char>();
        int left = 0;
        int best = 0;

        for (int right = 0; right < text.Length; right++)
        {
            while (window.Contains(text[right]))
            {
                window.Remove(text[left]);
                left++;
            }
            window.Add(text[right]);
            best = Math.Max(best, right - left + 1);
        }
        return best;
    }

    [Approach(Name = "Sliding Window, Jump Ahead", Time = "O(n)", Space = "O(k)", Idea = """
        Instead of removing characters one by one, remember where each character was **last seen**.
        On a repeat, jump `left` straight past that earlier position.
        (Only jump forward: an old position before `left` doesn't matter.)
        """)]
    public static int LongestWithLastSeen(string text)
    {
        var lastSeen = new Dictionary<char, int>();
        int left = 0;
        int best = 0;

        for (int right = 0; right < text.Length; right++)
        {
            char letter = text[right];
            if (lastSeen.ContainsKey(letter) && lastSeen[letter] >= left)
            {
                left = lastSeen[letter] + 1;
            }
            lastSeen[letter] = right;
            best = Math.Max(best, right - left + 1);
        }
        return best;
    }

    public static Example[] Examples =>
    [
        new(["abcabcbb"], 3),
        new(["bbbbb"], 1),
        new(["pwwkew"], 3),
        new(["abba"], 2),
    ];
}
