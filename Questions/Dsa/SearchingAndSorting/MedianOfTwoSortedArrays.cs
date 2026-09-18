namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 13, Title = "Median of Two Sorted Arrays", Level = Hard, Problem = """
    Two arrays are sorted. Find the median of all their numbers together: the middle value, or the average of the two middle values.
    `[1, 3]` and `[2]` → `2`. `[1, 2]` and `[3, 4]` → `2.5`.
    """)]
public static class MedianOfTwoSortedArrays
{
    [Approach(Name = "Merge Until the Middle", Time = "O(m + n)", Space = "O(1)", Idea = """
        Walk both arrays like the merge step of merge sort, without storing the merged result.
        Stop at the middle position and remember the last one or two values seen.
        """)]
    public static double MedianByMerging(int[] first, int[] second)
    {
        int total = first.Length + second.Length;
        int i = 0;
        int j = 0;
        int previous = 0;
        int current = 0;

        for (int step = 0; step <= total / 2; step++)
        {
            previous = current;
            if (j >= second.Length || (i < first.Length && first[i] <= second[j]))
            {
                current = first[i];
                i++;
            }
            else
            {
                current = second[j];
                j++;
            }
        }
        return total % 2 == 1 ? current : (previous + current) / 2.0;
    }

    [Approach(Name = "Binary Search the Split", Time = "O(log(min(m, n)))", Space = "O(1)", Idea = """
        Split both arrays so the left parts together hold half of all numbers. The split is correct when
        everything on the left is ≤ everything on the right:
        `firstLeft ≤ secondRight` and `secondLeft ≤ firstRight`.

        Binary search how many items to take from the shorter array. Too many taken from it → take fewer; too few → take more.
        Once correct, the median comes from the four numbers around the split.
        """)]
    public static double MedianByBinarySearch(int[] first, int[] second)
    {
        if (first.Length > second.Length)
        {
            return MedianByBinarySearch(second, first);
        }

        int m = first.Length;
        int n = second.Length;
        int half = (m + n + 1) / 2;
        int low = 0;
        int high = m;

        while (true)
        {
            int takeFromFirst = (low + high) / 2;
            int takeFromSecond = half - takeFromFirst;

            int firstLeft = takeFromFirst > 0 ? first[takeFromFirst - 1] : int.MinValue;
            int firstRight = takeFromFirst < m ? first[takeFromFirst] : int.MaxValue;
            int secondLeft = takeFromSecond > 0 ? second[takeFromSecond - 1] : int.MinValue;
            int secondRight = takeFromSecond < n ? second[takeFromSecond] : int.MaxValue;

            if (firstLeft > secondRight)
            {
                high = takeFromFirst - 1;
            }
            else if (secondLeft > firstRight)
            {
                low = takeFromFirst + 1;
            }
            else if ((m + n) % 2 == 1)
            {
                return Math.Max(firstLeft, secondLeft);
            }
            else
            {
                return (Math.Max(firstLeft, secondLeft) + (double)Math.Min(firstRight, secondRight)) / 2;
            }
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 3 }, new[] { 2 }], 2.0),
        new([new[] { 1, 2 }, new[] { 3, 4 }], 2.5),
        new([new[] { 1, 5, 9, 12 }, new[] { 2, 3, 4, 20, 21 }], 5.0),
    ];
}
