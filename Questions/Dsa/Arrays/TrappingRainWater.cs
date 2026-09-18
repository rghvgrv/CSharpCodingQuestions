namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_17, "Trapping Rain Water", Hard,
"Given bar heights, compute how much rain water is trapped between the bars.")]
public static class TrappingRainWater
{
    // Water above bar i = min(maxLeft, maxRight) - height[i].
    // Two pointers, Time O(n), Space O(1): the side with the smaller max is already decided.
    public static int Solve(int[] h)
    {
        int i = 0, j = h.Length - 1, leftMax = 0, rightMax = 0, water = 0;
        while (i < j)
        {
            if (h[i] < h[j])
            {
                leftMax = Math.Max(leftMax, h[i]);
                water += leftMax - h[i++];
            }
            else
            {
                rightMax = Math.Max(rightMax, h[j]);
                water += rightMax - h[j--];
            }
        }
        return water;
    }

    public static void Run()
    {
        Check("[0,1,0,2,1,0,1,3,2,1,2,1]", Solve([0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]), 6);
        Check("[4,2,0,3,2,5]", Solve([4, 2, 0, 3, 2, 5]), 9);
    }
}
