namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 8, Title = "Insertion Sort", Level = Easy, Problem = """
    Sort the array using insertion sort: like sorting playing cards in your hand, take each new card and slide it into its place.
    """)]
public static class InsertionSort
{
    [Approach(Name = "Shift and Insert", Time = "O(n²), O(n) if nearly sorted", Space = "O(1)", Idea = """
        The left part of the array is always sorted. For each next item (`current`):

        1. Shift every bigger item in the sorted part one step right.
        2. Drop `current` into the gap.

        It's very fast on nearly sorted data, which is why .NET's `Array.Sort` uses it for small pieces.
        """)]
    public static int[] InsertionSortArray(int[] numbers)
    {
        for (int i = 1; i < numbers.Length; i++)
        {
            int current = numbers[i];
            int j = i - 1;
            while (j >= 0 && numbers[j] > current)
            {
                numbers[j + 1] = numbers[j];
                j--;
            }
            numbers[j + 1] = current;
        }
        return numbers;
    }

    public static Example[] Examples =>
    [
        new([new[] { 12, 11, 13, 5, 6 }], new[] { 5, 6, 11, 12, 13 }),
        new([new[] { 2, 1 }], new[] { 1, 2 }),
    ];
}
