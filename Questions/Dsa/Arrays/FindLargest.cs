namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 1, Title = "Find the Largest Number", Level = Easy, Problem = """
    Return the largest number in a non-empty array. `[3, 9, 2, 7]` → `9`.
    """)]
public static class FindLargest
{
    [Approach(Name = "Sort, Then Take the Last", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort a copy of the array; the largest number ends up last.
        It works, but sorting does far more work than needed. We only want one number.
        """)]
    public static int FindLargestBySorting(int[] numbers)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        return sorted[sorted.Length - 1];
    }

    [Approach(Name = "One Pass", Time = "O(n)", Space = "O(1)", Idea = """
        Remember the biggest number seen so far. Look at each number once, and if it's bigger, remember it instead.
        """)]
    public static int FindLargestInOnePass(int[] numbers)
    {
        int largest = numbers[0];
        foreach (int number in numbers)
        {
            if (number > largest)
            {
                largest = number;
            }
        }
        return largest;
    }

    public static Example[] Examples =>
    [
        new([new[] { 3, 9, 2, 7 }], 9),
        new([new[] { -5, -1, -8 }], -1),
        new([new[] { 4 }], 4),
    ];
}
