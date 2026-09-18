namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 1, Title = "Binary Search", Level = Easy, Problem = """
    The array is sorted. Return the index of `target`, or `-1` if it isn't there.
    `numbers = [-1, 0, 3, 5, 9, 12]`, `target = 9` → `4`.
    """)]
public static class BinarySearch
{
    [Approach(Name = "Linear Search", Time = "O(n)", Space = "O(1)", Idea = """
        Check every item from the start. It works even on unsorted arrays, but it ignores the sorting.
        """)]
    public static int LinearSearch(int[] numbers, int target)
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

    [Approach(Name = "Binary Search (Recursive)", Time = "O(log n)", Space = "O(log n)", Idea = """
        Look at the middle item:

        - equal → found it
        - target is bigger → it can only be in the **right** half
        - target is smaller → it can only be in the **left** half

        Each step throws away half, so 1,000,000 items need only about 20 steps.
        The recursion uses stack space for each halving.
        """)]
    public static int BinarySearchRecursive(int[] numbers, int target)
    {
        return SearchRange(numbers, target, 0, numbers.Length - 1);
    }

    private static int SearchRange(int[] numbers, int target, int left, int right)
    {
        if (left > right)
        {
            return -1;
        }

        int middle = left + (right - left) / 2;
        if (numbers[middle] == target)
        {
            return middle;
        }
        if (numbers[middle] < target)
        {
            return SearchRange(numbers, target, middle + 1, right);
        }
        return SearchRange(numbers, target, left, middle - 1);
    }

    [Approach(Name = "Binary Search (Loop)", Time = "O(log n)", Space = "O(1)", Idea = """
        Same halving, with a loop instead of recursion, so no extra memory.
        `left + (right - left) / 2` is used instead of `(left + right) / 2`, which can overflow for huge indexes.
        """)]
    public static int BinarySearchLoop(int[] numbers, int target)
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
            if (numbers[middle] < target)
            {
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }
        return -1;
    }

    public static Example[] Examples =>
    [
        new([new[] { -1, 0, 3, 5, 9, 12 }, 9], 4),
        new([new[] { -1, 0, 3, 5, 9, 12 }, 2], -1),
        new([new[] { 5 }, 5], 0),
    ];
}
