namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 2, Title = "First and Last Position of a Value", Level = Medium, Problem = """
    The sorted array may contain `target` several times. Return `[first index, last index]`, or `[-1, -1]` if it's missing.
    `numbers = [5, 7, 7, 8, 8, 10]`, `target = 8` → `[3, 4]`.
    """)]
public static class FirstAndLastPosition
{
    [Approach(Name = "Scan the Array", Time = "O(n)", Space = "O(1)", Idea = """
        Walk the array once. Remember the first time `target` appears, and keep updating the last time.
        """)]
    public static int[] FindRangeLinear(int[] numbers, int target)
    {
        int first = -1;
        int last = -1;
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] == target)
            {
                if (first == -1)
                {
                    first = i;
                }
                last = i;
            }
        }
        return new int[] { first, last };
    }

    [Approach(Name = "Two Binary Searches", Time = "O(log n)", Space = "O(1)", Idea = """
        Run binary search twice. When you find `target`, don't stop:

        - to find the **first** position, remember it and keep searching the **left** half
        - to find the **last** position, remember it and keep searching the **right** half
        """)]
    public static int[] FindRangeBinary(int[] numbers, int target)
    {
        int first = FindEdge(numbers, target, searchLeft: true);
        int last = FindEdge(numbers, target, searchLeft: false);
        return new int[] { first, last };
    }

    private static int FindEdge(int[] numbers, int target, bool searchLeft)
    {
        int left = 0;
        int right = numbers.Length - 1;
        int found = -1;
        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            if (numbers[middle] == target)
            {
                found = middle;
                if (searchLeft)
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }
            else if (numbers[middle] < target)
            {
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }
        return found;
    }

    public static Example[] Examples =>
    [
        new([new[] { 5, 7, 7, 8, 8, 10 }, 8], new[] { 3, 4 }),
        new([new[] { 5, 7, 7, 8, 8, 10 }, 6], new[] { -1, -1 }),
        new([new[] { 2, 2, 2, 2 }, 2], new[] { 0, 3 }),
    ];
}
