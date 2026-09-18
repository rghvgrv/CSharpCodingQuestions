namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 1, Title = "Valid Anagram", Level = Easy, Problem = """
    Two words are anagrams if they use exactly the same letters the same number of times, like `"listen"` and `"silent"`.
    Return `true` if `first` and `second` are anagrams. Only lowercase letters `a`–`z`.
    """)]
public static class ValidAnagram
{
    [Approach(Name = "Sort Both Words", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort the letters of each word. Anagrams become the same text: `"listen"` and `"silent"` both become `"eilnst"`.
        """)]
    public static bool IsAnagramBySorting(string first, string second)
    {
        char[] a = first.ToCharArray();
        char[] b = second.ToCharArray();
        Array.Sort(a);
        Array.Sort(b);
        return new string(a) == new string(b);
    }

    [Approach(Name = "Count Letters", Time = "O(n)", Space = "O(1)", Idea = """
        Use an array of 26 counters, one per letter. Add 1 for each letter of `first` and subtract 1 for each letter of `second`.
        If every counter ends at 0, the words have the same letters.
        `letter - 'a'` turns `'a'` into 0, `'b'` into 1, and so on.
        """)]
    public static bool IsAnagramByCounting(string first, string second)
    {
        if (first.Length != second.Length)
        {
            return false;
        }

        int[] counts = new int[26];
        for (int i = 0; i < first.Length; i++)
        {
            counts[first[i] - 'a']++;
            counts[second[i] - 'a']--;
        }

        foreach (int count in counts)
        {
            if (count != 0)
            {
                return false;
            }
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new(["listen", "silent"], true),
        new(["anagram", "nagaram"], true),
        new(["rat", "car"], false),
    ];
}
