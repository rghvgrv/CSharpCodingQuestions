namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 13, Title = "Sort 0s, 1s and 2s", Level = Medium, Problem = """
    The array contains only `0`, `1` and `2`. Sort it.
    `[2, 0, 2, 1, 1, 0]` → `[0, 0, 1, 1, 2, 2]`.
    """)]
public static class SortColors
{
    [Approach(Name = "Built-in Sort", Time = "O(n log n)", Space = "O(log n)", Idea = """
        `Array.Sort` works for any numbers, but it doesn't use the fact that there are only three different values.
        """)]
    public static int[] SortWithBuiltIn(int[] numbers)
    {
        Array.Sort(numbers);
        return numbers;
    }

    [Approach(Name = "Count, Then Rewrite", Time = "O(n)", Space = "O(1)", Idea = """
        1. Count how many 0s, 1s and 2s there are.
        2. Overwrite the array: that many 0s, then that many 1s, then 2s.

        Two passes over the array.
        """)]
    public static int[] SortByCounting(int[] numbers)
    {
        int[] counts = new int[3];
        foreach (int number in numbers)
        {
            counts[number]++;
        }

        int index = 0;
        for (int value = 0; value <= 2; value++)
        {
            for (int i = 0; i < counts[value]; i++)
            {
                numbers[index] = value;
                index++;
            }
        }
        return numbers;
    }

    [Approach(Name = "Dutch National Flag (One Pass)", Time = "O(n)", Space = "O(1)", Idea = """
        Keep three regions with three pointers:

        - everything before `low` is 0
        - everything after `high` is 2
        - `middle` scans the unknown part in between

        At `middle`: a 0 is swapped to `low`, a 1 stays where it is, and a 2 is swapped to `high`.
        After swapping in from `high`, don't move `middle`, because the value that arrived hasn't been checked yet.
        """)]
    public static int[] SortInOnePass(int[] numbers)
    {
        int low = 0;
        int middle = 0;
        int high = numbers.Length - 1;

        while (middle <= high)
        {
            if (numbers[middle] == 0)
            {
                Swap(numbers, low, middle);
                low++;
                middle++;
            }
            else if (numbers[middle] == 1)
            {
                middle++;
            }
            else
            {
                Swap(numbers, middle, high);
                high--;
            }
        }
        return numbers;
    }

    private static void Swap(int[] numbers, int i, int j)
    {
        int temp = numbers[i];
        numbers[i] = numbers[j];
        numbers[j] = temp;
    }

    public static Example[] Examples =>
    [
        new([new[] { 2, 0, 2, 1, 1, 0 }], new[] { 0, 0, 1, 1, 2, 2 }),
        new([new[] { 2, 0, 1 }], new[] { 0, 1, 2 }),
    ];
}
