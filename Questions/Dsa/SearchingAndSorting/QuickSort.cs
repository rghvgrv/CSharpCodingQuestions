namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 10, Title = "Quick Sort", Level = Medium, Problem = """
    Sort the array using quick sort: pick a **pivot**, move smaller items to its left and bigger items to its right, then sort each side.
    """)]
public static class QuickSort
{
    [Approach(Name = "Last Item as Pivot", Time = "O(n log n) average, O(n²) worst", Space = "O(log n)", Idea = """
        **Partition**: use the last item as the pivot. Walk through the range and move every item smaller than the pivot to the front.
        Then put the pivot right after them. It's now in its final place.
        Recursively sort the part left of the pivot and the part right of it.

        Weakness: on an already sorted array the pivot is always the biggest item, so one side is empty and it degrades to `O(n²)`.
        """)]
    public static int[] QuickSortLastPivot(int[] numbers)
    {
        SortRange(numbers, 0, numbers.Length - 1);
        return numbers;
    }

    private static void SortRange(int[] numbers, int low, int high)
    {
        if (low >= high)
        {
            return;
        }

        int pivot = numbers[high];
        int smallerCount = low;
        for (int i = low; i < high; i++)
        {
            if (numbers[i] < pivot)
            {
                (numbers[i], numbers[smallerCount]) = (numbers[smallerCount], numbers[i]);   // swap
                smallerCount++;
            }
        }
        (numbers[smallerCount], numbers[high]) = (numbers[high], numbers[smallerCount]);   // pivot to its final place

        SortRange(numbers, low, smallerCount - 1);
        SortRange(numbers, smallerCount + 1, high);
    }

    [Approach(Name = "Random Pivot", Time = "O(n log n) expected", Space = "O(log n)", Idea = """
        Same algorithm, with one extra line: before partitioning, swap a **randomly chosen** item into the pivot spot.
        Bad splits become extremely unlikely, whatever the order of the input.
        """)]
    public static int[] QuickSortRandomPivot(int[] numbers)
    {
        SortRangeRandom(numbers, 0, numbers.Length - 1);
        return numbers;
    }

    private static void SortRangeRandom(int[] numbers, int low, int high)
    {
        if (low >= high)
        {
            return;
        }

        int randomIndex = Random.Shared.Next(low, high + 1);
        (numbers[randomIndex], numbers[high]) = (numbers[high], numbers[randomIndex]);   // the new line

        int pivot = numbers[high];
        int smallerCount = low;
        for (int i = low; i < high; i++)
        {
            if (numbers[i] < pivot)
            {
                (numbers[i], numbers[smallerCount]) = (numbers[smallerCount], numbers[i]);
                smallerCount++;
            }
        }
        (numbers[smallerCount], numbers[high]) = (numbers[high], numbers[smallerCount]);

        SortRangeRandom(numbers, low, smallerCount - 1);
        SortRangeRandom(numbers, smallerCount + 1, high);
    }

    public static Example[] Examples =>
    [
        new([new[] { 10, 7, 8, 9, 1, 5 }], new[] { 1, 5, 7, 8, 9, 10 }),
        new([new[] { 3, 3, 1, 1, 2, 2 }], new[] { 1, 1, 2, 2, 3, 3 }),
    ];
}
