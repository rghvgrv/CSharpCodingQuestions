namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 3, Title = "Search in a Rotated Sorted Array", Level = Medium, Problem = """
    A sorted array of different numbers was rotated, so part of the beginning moved to the end: `[0, 1, 2, 4, 5, 6, 7]` became `[4, 5, 6, 7, 0, 1, 2]`.
    Return the index of `target`, or `-1`.
    """)]
public static class SearchRotatedArray
{
    [Approach(Name = "Linear Search", Time = "O(n)", Space = "O(1)", Idea = """
        Check every item. Simple, but it doesn't use the fact that the array is almost sorted.
        """)]
    public static int SearchLinear(int[] numbers, int target)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] == target)
            {
                return i;
            }
        }
        return -1;
    }

    [Approach(Name = "Modified Binary Search", Time = "O(log n)", Space = "O(1)", Idea = """
        Cut the array at the middle. At least **one half is always normally sorted**, and you can tell which by comparing its ends.

        - If the left half is sorted and `target` lies between its ends, search left; otherwise search right.
        - If the right half is sorted and `target` lies between its ends, search right; otherwise search left.
        """)]
    public static int SearchBinary(int[] numbers, int target)
    {
        int left = 0;
        int right = numbers.Length - 1;
        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            if (numbers[middle] == target)
            {
                return middle;
            }

            bool leftHalfIsSorted = numbers[left] <= numbers[middle];
            if (leftHalfIsSorted)
            {
                if (numbers[left] <= target && target < numbers[middle])
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }
            else
            {
                if (numbers[middle] < target && target <= numbers[right])
                {
                    left = middle + 1;
                }
                else
                {
                    right = middle - 1;
                }
            }
        }
        return -1;
    }

    public static Example[] Examples =>
    [
        new([new[] { 4, 5, 6, 7, 0, 1, 2 }, 0], 4),
        new([new[] { 4, 5, 6, 7, 0, 1, 2 }, 3], -1),
        new([new[] { 3, 1 }, 1], 1),
    ];
}
