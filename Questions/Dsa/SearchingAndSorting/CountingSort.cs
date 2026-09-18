namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 12, Title = "Counting Sort", Level = Medium, Problem = """
    Sort numbers that come from a small range (like ages 0–120) **without comparing** them to each other.
    """)]
public static class CountingSort
{
    [Approach(Name = "Count, Then Rebuild", Time = "O(n + k)", Space = "O(k)", Idea = """
        1. Find the smallest and largest values. `k` is the size of that range.
        2. Count how many times each value appears (`counts[value - min]++`).
        3. Rebuild the array: each value, repeated as many times as it was counted, from small to large.

        When `k` is small, this beats every comparison sort, which can't do better than `O(n log n)`.
        """)]
    public static int[] CountingSortArray(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            return numbers;
        }

        int min = numbers.Min();
        int max = numbers.Max();
        int[] counts = new int[max - min + 1];
        foreach (int number in numbers)
        {
            counts[number - min]++;
        }

        int index = 0;
        for (int offset = 0; offset < counts.Length; offset++)
        {
            for (int repeat = 0; repeat < counts[offset]; repeat++)
            {
                numbers[index] = offset + min;
                index++;
            }
        }
        return numbers;
    }

    public static Example[] Examples =>
    [
        new([new[] { 4, 2, 2, 8, 3, 3, 1 }], new[] { 1, 2, 2, 3, 3, 4, 8 }),
        new([new[] { 0, -3, 5, -3 }], new[] { -3, -3, 0, 5 }),
    ];
}
