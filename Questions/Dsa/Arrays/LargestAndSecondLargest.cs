namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_01, "Largest & Second Largest", Easy,
"Find the largest and the second largest distinct values of an array in a single pass. Return -1 for second largest if it doesn't exist.")]
public static class LargestAndSecondLargest
{
    // Time O(n), Space O(1)
    public static (int Largest, int Second) Solve(int[] nums)
    {
        int first = int.MinValue, second = -1;
        bool hasSecond = false;
        foreach (int x in nums)
        {
            if (x > first)
            {
                if (first != int.MinValue) { second = first; hasSecond = true; }
                first = x;
            }
            else if (x < first && (!hasSecond || x > second))
            {
                second = x;
                hasSecond = true;
            }
        }
        return (first, hasSecond ? second : -1);
    }

    public static void Run()
    {
        Check("[12, 35, 1, 10, 34, 1]", Solve([12, 35, 1, 10, 34, 1]), (35, 34));
        Check("[10, 5, 10]", Solve([10, 5, 10]), (10, 5));
        Check("[7, 7, 7]", Solve([7, 7, 7]), (7, -1));
    }
}
