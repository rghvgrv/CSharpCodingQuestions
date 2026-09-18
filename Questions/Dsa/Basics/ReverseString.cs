namespace CodingQuestions.Dsa.Basics;

[Q(1_01_01, "Reverse a String", Easy,
"Reverse a string without using built-in Reverse(). Show both the two-pointer swap and the recursive approach.")]
public static class ReverseString
{
    // Time O(n), Space O(n) for the char array. Swap from both ends toward the middle.
    public static string TwoPointer(string s)
    {
        var chars = s.ToCharArray();
        for (int i = 0, j = chars.Length - 1; i < j; i++, j--)
            (chars[i], chars[j]) = (chars[j], chars[i]);
        return new string(chars);
    }

    // Time O(n²) due to string concatenation. Shown to practice recursion, not for production.
    public static string Recursive(string s) => s.Length <= 1 ? s : Recursive(s[1..]) + s[0];

    public static void Run()
    {
        Check("TwoPointer(\"hello\")", TwoPointer("hello"), "olleh");
        Check("TwoPointer(\"C# rocks\")", TwoPointer("C# rocks"), "skcor #C");
        Check("TwoPointer(\"\")", TwoPointer(""), "");
        Check("Recursive(\"abcde\")", Recursive("abcde"), "edcba");
    }
}
