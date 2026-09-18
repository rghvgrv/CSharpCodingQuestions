namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 18, Title = "Trapping Rain Water", Level = Hard, Problem = """
    `heights` are bars of width 1. After rain, water gets trapped between taller bars. How much water is trapped?
    `[0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]` → `6`.
    """)]
public static class TrappingRainWater
{
    [Approach(Name = "For Each Bar, Look Both Ways", Time = "O(n²)", Space = "O(1)", Idea = """
        Water above bar `i` = `min(tallest bar on the left, tallest bar on the right) - heights[i]`.
        For every bar, scan left and right to find those two tallest bars.
        """)]
    public static int TrapBruteForce(int[] heights)
    {
        int water = 0;
        for (int i = 0; i < heights.Length; i++)
        {
            int leftMax = 0;
            for (int j = 0; j <= i; j++)
            {
                leftMax = Math.Max(leftMax, heights[j]);
            }

            int rightMax = 0;
            for (int j = i; j < heights.Length; j++)
            {
                rightMax = Math.Max(rightMax, heights[j]);
            }

            water += Math.Min(leftMax, rightMax) - heights[i];
        }
        return water;
    }

    [Approach(Name = "Precompute Left and Right Maximums", Time = "O(n)", Space = "O(n)", Idea = """
        Same formula, without rescanning:

        1. `leftMax[i]` = tallest bar from the start up to `i` (one pass left → right).
        2. `rightMax[i]` = tallest bar from `i` to the end (one pass right → left).
        3. Add up `min(leftMax[i], rightMax[i]) - heights[i]`.
        """)]
    public static int TrapWithArrays(int[] heights)
    {
        int n = heights.Length;
        int[] leftMax = new int[n];
        int[] rightMax = new int[n];

        for (int i = 0; i < n; i++)
        {
            leftMax[i] = Math.Max(i > 0 ? leftMax[i - 1] : 0, heights[i]);
        }
        for (int i = n - 1; i >= 0; i--)
        {
            rightMax[i] = Math.Max(i < n - 1 ? rightMax[i + 1] : 0, heights[i]);
        }

        int water = 0;
        for (int i = 0; i < n; i++)
        {
            water += Math.Min(leftMax[i], rightMax[i]) - heights[i];
        }
        return water;
    }

    [Approach(Name = "Two Pointers", Time = "O(n)", Space = "O(1)", Idea = """
        Walk inward from both ends, keeping `leftMax` and `rightMax`.
        Whichever side has the **lower** wall is decided: the water there depends only on its own max,
        because the other side is known to have something at least as tall.
        So add that side's water and move that pointer.
        """)]
    public static int TrapTwoPointers(int[] heights)
    {
        int left = 0;
        int right = heights.Length - 1;
        int leftMax = 0;
        int rightMax = 0;
        int water = 0;

        while (left < right)
        {
            if (heights[left] < heights[right])
            {
                leftMax = Math.Max(leftMax, heights[left]);
                water += leftMax - heights[left];
                left++;
            }
            else
            {
                rightMax = Math.Max(rightMax, heights[right]);
                water += rightMax - heights[right];
                right--;
            }
        }
        return water;
    }

    public static Example[] Examples =>
    [
        new([new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 }], 6),
        new([new[] { 4, 2, 0, 3, 2, 5 }], 9),
    ];
}
