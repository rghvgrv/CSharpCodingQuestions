namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 2, Title = "Palindrome Check", Level = Easy, Problem = """
    A palindrome reads the same forwards and backwards, like `"racecar"` or `"Level"`.
    Return `true` if the word is a palindrome. Upper and lower case count as the same letter.
    """)]
public static class PalindromeCheck
{
    [Approach(Name = "Reverse and Compare", Time = "O(n)", Space = "O(n)", Idea = """
        Make a reversed copy of the word and check whether it equals the original.
        Simple, but it builds a whole new string just to compare.
        """)]
    public static bool IsPalindromeByReversing(string word)
    {
        string lower = word.ToLower();
        char[] letters = lower.ToCharArray();
        Array.Reverse(letters);
        string reversed = new string(letters);
        return lower == reversed;
    }

    [Approach(Name = "Two Pointers", Time = "O(n)", Space = "O(1)", Idea = """
        Compare the first letter with the last, the second with the second-last, and so on.
        Stop at the first mismatch. No copy is needed, so it uses no extra memory.
        """)]
    public static bool IsPalindromeWithTwoPointers(string word)
    {
        int left = 0;
        int right = word.Length - 1;

        while (left < right)
        {
            if (char.ToLower(word[left]) != char.ToLower(word[right]))
            {
                return false;
            }
            left++;
            right--;
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new(["racecar"], true),
        new(["Level"], true),
        new(["hello"], false),
    ];
}
