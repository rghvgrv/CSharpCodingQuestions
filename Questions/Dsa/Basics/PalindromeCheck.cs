namespace CodingQuestions.Dsa.Basics;

[Q(1_01_02, "Palindrome Check", Easy,
"Check whether a string reads the same forwards and backwards, ignoring case and non-alphanumeric characters. Also check whether an integer is a palindrome without converting it to a string.")]
public static class PalindromeCheck
{
    // Time O(n), Space O(1): two pointers that skip characters we ignore.
    public static bool IsPalindrome(string s)
    {
        int i = 0, j = s.Length - 1;
        while (i < j)
        {
            if (!char.IsLetterOrDigit(s[i])) i++;
            else if (!char.IsLetterOrDigit(s[j])) j--;
            else if (char.ToLower(s[i++]) != char.ToLower(s[j--])) return false;
        }
        return true;
    }

    // Time O(digits): rebuild the number backwards and compare.
    public static bool IsPalindrome(int x)
    {
        if (x < 0) return false;
        int original = x, reversed = 0;
        while (x > 0)
        {
            reversed = reversed * 10 + x % 10;
            x /= 10;
        }
        return original == reversed;
    }

    public static void Run()
    {
        Check("\"A man, a plan, a canal: Panama\"", IsPalindrome("A man, a plan, a canal: Panama"), true);
        Check("\"race a car\"", IsPalindrome("race a car"), false);
        Check("121", IsPalindrome(121), true);
        Check("-121", IsPalindrome(-121), false);
        Check("10", IsPalindrome(10), false);
    }
}
