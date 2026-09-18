namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_08, "Unique Paths & Minimum Path Sum (Grid DP)", Medium,
"A robot moves only right or down on an m×n grid. (1) Count paths from top-left to bottom-right, with obstacles (1). (2) Find the path with the minimum sum.")]
public static class UniquePaths
{
    // paths[r, c] = paths from above + paths from the left. An obstacle cell has 0 paths.
    // One row of memory is enough: row[c] += row[c - 1].
    public static int CountPaths(int[][] grid)
    {
        int cols = grid[0].Length;
        var row = new int[cols];
        row[0] = 1;
        foreach (var cells in grid)
            for (int c = 0; c < cols; c++)
            {
                if (cells[c] == 1) row[c] = 0;
                else if (c > 0) row[c] += row[c - 1];
            }
        return row[^1];
    }

    public static int MinPathSum(int[][] g)
    {
        int rows = g.Length, cols = g[0].Length;
        var dp = new int[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                dp[r, c] = g[r][c] + (r, c) switch
                {
                    (0, 0) => 0,
                    (0, _) => dp[r, c - 1],
                    (_, 0) => dp[r - 1, c],
                    _ => Math.Min(dp[r - 1, c], dp[r, c - 1])
                };
        return dp[rows - 1, cols - 1];
    }

    public static void Run()
    {
        Check("3×7 empty grid", CountPaths(Enumerable.Range(0, 3).Select(_ => new int[7]).ToArray()), 28);
        Check("3×3 with center obstacle", CountPaths([[0, 0, 0], [0, 1, 0], [0, 0, 0]]), 2);
        Check("MinPathSum([[1,3,1],[1,5,1],[4,2,1]])", MinPathSum([[1, 3, 1], [1, 5, 1], [4, 2, 1]]), 7);
    }
}
