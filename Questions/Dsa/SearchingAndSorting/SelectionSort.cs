namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 7, Title = "Selection Sort", Level = Easy, Problem = """
    Sort the array using selection sort: repeatedly select the smallest remaining item and put it next in line.
    """)]
public static class SelectionSort
{
    [Approach(Name = "Select the Minimum", Time = "O(n²)", Space = "O(1)", Idea = """
        For each position `i` from the left:

        1. Find the smallest item from `i` to the end.
        2. Swap it into position `i`.

        It always does about n²/2 comparisons, even on sorted input, but at most `n - 1` swaps.
        That helps when writing to memory is expensive.
        """)]
    public static int[] SelectionSortArray(int[] numbers)
    {
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            int smallest = i;
            for (int j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[j] < numbers[smallest])
                {
                    smallest = j;
                }
            }

            int temp = numbers[i];
            numbers[i] = numbers[smallest];
            numbers[smallest] = temp;
        }
        return numbers;
    }

    public static Example[] Examples =>
    [
        new([new[] { 64, 25, 12, 22, 11 }], new[] { 11, 12, 22, 25, 64 }),
        new([new[] { 3, -1, 2 }], new[] { -1, 2, 3 }),
    ];
}
