namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 1, Title = "Reverse a String", Level = Easy, Problem = """
    Reverse a string: `"hello"` becomes `"olleh"`.
    """)]
public static class ReverseString
{
    [Approach(Name = "Add Characters One by One", Time = "O(n²)", Space = "O(n)", Idea = """
        Walk from the last character to the first, adding each one to a new string.

        Easy to read, but slow: a string can't be changed, so `result += letter` copies the whole string every time.
        """)]
    public static string ReverseByAdding(string text)
    {
        string result = "";
        for (int i = text.Length - 1; i >= 0; i--)
        {
            result += text[i];
        }
        return result;
    }

    [Approach(Name = "Two Pointers", Time = "O(n)", Space = "O(n)", Idea = """
        1. Copy the string into a `char[]`, which *can* be changed.
        2. Put one pointer on the first character and one on the last.
        3. Swap those two characters, then move both pointers one step toward the middle.
        4. When the pointers meet, every character has been swapped. Turn the array back into a string.
        """)]
    public static string ReverseWithTwoPointers(string text)
    {
        char[] letters = text.ToCharArray();
        int left = 0;
        int right = letters.Length - 1;

        while (left < right)
        {
            char temp = letters[left];
            letters[left] = letters[right];
            letters[right] = temp;

            left++;
            right--;
        }
        return new string(letters);
    }

    public static Example[] Examples =>
    [
        new(["hello"], "olleh"),
        new(["C# rocks"], "skcor #C"),
        new(["a"], "a"),
    ];
}
