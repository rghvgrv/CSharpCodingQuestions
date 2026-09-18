namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 16, Title = "Container With Most Water", Level = Medium, Problem = """
    `heights[i]` is a vertical wall at position `i`. Pick two walls. Together with the ground they hold water:
    `amount = (distance between them) × (height of the shorter wall)`. Return the largest amount possible.
    `[1, 8, 6, 2, 5, 4, 8, 3, 7]` → `49` (walls 8 and 7, distance 7).
    """)]
public static class ContainerWithMostWater
{
    [Approach(Name = "Try Every Pair", Time = "O(n²)", Space = "O(1)", Idea = """
        Compute the water for every pair of walls and keep the best.
        """)]
    public static int MaxWaterBruteForce(int[] heights)
    {
        int best = 0;
        for (int i = 0; i < heights.Length; i++)
        {
            for (int j = i + 1; j < heights.Length; j++)
            {
                int water = (j - i) * Math.Min(heights[i], heights[j]);
                best = Math.Max(best, water);
            }
        }
        return best;
    }

    [Approach(Name = "Two Pointers", Time = "O(n)", Space = "O(1)", Idea = """
        Start with the widest container: the first and last walls. Then move inward.

        Which wall should move? Always the **shorter** one. The shorter wall limits the water, so keeping it while the width shrinks can never give more.
        Moving it is the only chance to find a taller wall.
        """)]
    public static int MaxWaterTwoPointers(int[] heights)
    {
        int left = 0;
        int right = heights.Length - 1;
        int best = 0;

        while (left < right)
        {
            int water = (right - left) * Math.Min(heights[left], heights[right]);
            best = Math.Max(best, water);

            if (heights[left] < heights[right])
            {
                left++;
            }
            else
            {
                right--;
            }
        }
        return best;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 }], 49),
        new([new[] { 1, 1 }], 1),
        new([new[] { 4, 3, 2, 1, 4 }], 16),
    ];
}
