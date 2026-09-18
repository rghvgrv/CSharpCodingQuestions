namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 2, Title = "Second Largest Number", Level = Easy, Problem = """
    Return the second largest **different** number in the array, or `-1` if there isn't one.
    `[12, 35, 1, 10, 34]` → `34`. `[7, 7, 7]` → `-1`.
    """)]
public static class SecondLargest
{
    [Approach(Name = "Sort", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort a copy. The largest is at the end. Walk backwards to the first number that's smaller than it.
        """)]
    public static int SecondLargestBySorting(int[] numbers)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        int largest = sorted[sorted.Length - 1];
        for (int i = sorted.Length - 2; i >= 0; i--)
        {
            if (sorted[i] < largest)
            {
                return sorted[i];
            }
        }
        return -1;
    }

    [Approach(Name = "Two Passes", Time = "O(n)", Space = "O(1)", Idea = """
        1. First pass: find the largest number.
        2. Second pass: find the largest number that is **smaller** than it.
        """)]
    public static int SecondLargestTwoPasses(int[] numbers)
    {
        int largest = int.MinValue;
        foreach (int number in numbers)
        {
            if (number > largest)
            {
                largest = number;
            }
        }

        int second = -1;
        foreach (int number in numbers)
        {
            if (number < largest && number > second)
            {
                second = number;
            }
        }
        return second;
    }

    [Approach(Name = "One Pass", Time = "O(n)", Space = "O(1)", Idea = """
        Track the top two while scanning once:

        - A number bigger than `largest` pushes the old largest down to `second`.
        - A number between `second` and `largest` becomes the new `second`.
        - A number equal to `largest` is skipped, because we want a *different* value.
        """)]
    public static int SecondLargestOnePass(int[] numbers)
    {
        int largest = int.MinValue;
        int second = int.MinValue;
        foreach (int number in numbers)
        {
            if (number > largest)
            {
                second = largest;
                largest = number;
            }
            else if (number < largest && number > second)
            {
                second = number;
            }
        }
        return second == int.MinValue ? -1 : second;
    }

    public static Example[] Examples =>
    [
        new([new[] { 12, 35, 1, 10, 34, 1 }], 34),
        new([new[] { 10, 5, 10 }], 5),
        new([new[] { 7, 7, 7 }], -1),
    ];
}
