namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 5, Title = "Remove Duplicates from a Sorted Array", Level = Easy, Problem = """
    The array is sorted, so equal values sit next to each other. Keep each value once.
    `[0, 0, 1, 1, 1, 2, 3, 3]` → `[0, 1, 2, 3]`.
    """)]
public static class RemoveDuplicatesSorted
{
    [Approach(Name = "Copy Unique Values", Time = "O(n)", Space = "O(n)", Idea = """
        Walk through the array and add a value to a new list only when it differs from the previous value.
        """)]
    public static int[] RemoveDuplicatesWithList(int[] numbers)
    {
        var unique = new List<int>();
        for (int i = 0; i < numbers.Length; i++)
        {
            if (i == 0 || numbers[i] != numbers[i - 1])
            {
                unique.Add(numbers[i]);
            }
        }
        return unique.ToArray();
    }

    [Approach(Name = "Two Pointers In Place", Time = "O(n)", Space = "O(1)", Idea = """
        Use the front of the same array as the result:

        - `write` is where the next unique value goes.
        - `read` scans every item.
        - When `numbers[read]` differs from the last written value, copy it to `write` and move `write` forward.

        The first `write` items are the answer. (We return that part so you can see it.)
        """)]
    public static int[] RemoveDuplicatesInPlace(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            return numbers;
        }

        int write = 1;
        for (int read = 1; read < numbers.Length; read++)
        {
            if (numbers[read] != numbers[write - 1])
            {
                numbers[write] = numbers[read];
                write++;
            }
        }
        return numbers[..write];
    }

    public static Example[] Examples =>
    [
        new([new[] { 0, 0, 1, 1, 1, 2, 3, 3 }], new[] { 0, 1, 2, 3 }),
        new([new[] { 1, 1, 2 }], new[] { 1, 2 }),
        new([new[] { 5 }], new[] { 5 }),
    ];
}
