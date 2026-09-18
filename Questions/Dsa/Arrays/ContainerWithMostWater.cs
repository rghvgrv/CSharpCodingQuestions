namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_15, "Container With Most Water", Medium,
"height[i] is a vertical line at x = i. Pick two lines that, with the x-axis, hold the most water. Return that amount.")]
public static class ContainerWithMostWater
{
    // Two pointers, Time O(n): the shorter line limits the area, so moving the taller one can never help.
    public static int Solve(int[] height)
    {
        int i = 0, j = height.Length - 1, best = 0;
        while (i < j)
        {
            best = Math.Max(best, Math.Min(height[i], height[j]) * (j - i));
            if (height[i] < height[j]) i++; else j--;
        }
        return best;
    }

    public static void Run()
    {
        Check("[1,8,6,2,5,4,8,3,7]", Solve([1, 8, 6, 2, 5, 4, 8, 3, 7]), 49);
        Check("[1,1]", Solve([1, 1]), 1);
    }
}
