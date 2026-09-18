namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 4, Title = "Find a Peak Element", Level = Medium, Problem = """
    A peak is an item bigger than both its neighbors (outside the array counts as very small). Neighbors are never equal.
    Return the index of any peak. `[1, 2, 3, 1]` → `2`.
    """)]
public static class FindPeakElement
{
    [Approach(Name = "Linear Scan", Time = "O(n)", Space = "O(1)", Idea = """
        Walk forward while the numbers go up. The first time the next number is smaller, you're on a peak.
        If they go up all the way, the last item is the peak.
        """)]
    public static int FindPeakLinear(int[] numbers)
    {
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            if (numbers[i] > numbers[i + 1])
            {
                return i;
            }
        }
        return numbers.Length - 1;
    }

    [Approach(Name = "Binary Search on the Slope", Time = "O(log n)", Space = "O(1)", Idea = """
        Look at the middle and its right neighbor:

        - If the numbers go **up** to the right, a peak must exist on the right side, because the climb has to stop somewhere.
        - Otherwise a peak is at the middle or to its left.

        Each step keeps the half that is guaranteed to hold a peak.
        """)]
    public static int FindPeakBinary(int[] numbers)
    {
        int left = 0;
        int right = numbers.Length - 1;
        while (left < right)
        {
            int middle = left + (right - left) / 2;
            if (numbers[middle] < numbers[middle + 1])
            {
                left = middle + 1;
            }
            else
            {
                right = middle;
            }
        }
        return left;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 1 }], 2),
        new([new[] { 5, 4, 3 }], 0),
        new([new[] { 1, 3, 5, 7 }], 3),
    ];
}
