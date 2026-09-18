namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 6, Title = "Bubble Sort", Level = Easy, Problem = """
    Sort the array from smallest to largest using bubble sort: compare neighbors and swap them when they're in the wrong order.
    """)]
public static class BubbleSort
{
    [Approach(Name = "Basic Bubble Sort", Time = "O(n²)", Space = "O(1)", Idea = """
        Walk through the array, swapping each pair of neighbors that's out of order.
        After one pass, the largest item has "bubbled up" to the end. Repeat `n - 1` times.
        """)]
    public static int[] BubbleSortBasic(int[] numbers)
    {
        for (int pass = 0; pass < numbers.Length - 1; pass++)
        {
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                if (numbers[i] > numbers[i + 1])
                {
                    int temp = numbers[i];
                    numbers[i] = numbers[i + 1];
                    numbers[i + 1] = temp;
                }
            }
        }
        return numbers;
    }

    [Approach(Name = "Optimized: Shorter Passes and Early Exit", Time = "O(n²), O(n) if already sorted", Space = "O(1)", Idea = """
        Two small improvements:

        1. After each pass the biggest items are already in place at the end, so don't compare them again.
        2. If a whole pass makes no swap, the array is sorted, so stop.
        """)]
    public static int[] BubbleSortOptimized(int[] numbers)
    {
        for (int pass = 0; pass < numbers.Length - 1; pass++)
        {
            bool swapped = false;
            for (int i = 0; i < numbers.Length - 1 - pass; i++)
            {
                if (numbers[i] > numbers[i + 1])
                {
                    int temp = numbers[i];
                    numbers[i] = numbers[i + 1];
                    numbers[i + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped)
            {
                break;
            }
        }
        return numbers;
    }

    public static Example[] Examples =>
    [
        new([new[] { 5, 1, 4, 2, 8 }], new[] { 1, 2, 4, 5, 8 }),
        new([new[] { 1, 2, 3 }], new[] { 1, 2, 3 }),
    ];
}
