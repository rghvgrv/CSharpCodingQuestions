namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_03, "Number of Islands (Grid DFS)", Medium,
"In a grid of '1' (land) and '0' (water), count the islands. An island is land connected up/down/left/right. Also find the largest island's area.")]
public static class NumberOfIslands
{
    // A grid is a graph: each cell links to its 4 neighbors. Flood-fill each new island to "sink" it. Time O(rows·cols)
    public static (int Count, int MaxArea) Solve(string[] rows)
    {
        var grid = rows.Select(r => r.ToCharArray()).ToArray();
        int count = 0, maxArea = 0;

        int Sink(int r, int c)
        {
            if (r < 0 || c < 0 || r >= grid.Length || c >= grid[0].Length || grid[r][c] != '1') return 0;
            grid[r][c] = '0';
            return 1 + Sink(r + 1, c) + Sink(r - 1, c) + Sink(r, c + 1) + Sink(r, c - 1);
        }

        for (int r = 0; r < grid.Length; r++)
            for (int c = 0; c < grid[0].Length; c++)
                if (grid[r][c] == '1')
                {
                    count++;
                    maxArea = Math.Max(maxArea, Sink(r, c));
                }
        return (count, maxArea);
    }

    public static void Run()
    {
        Check("grid 1", Solve(["11110", "11010", "11000", "00000"]), (1, 9));
        Check("grid 2", Solve(["11000", "11000", "00100", "00011"]), (3, 4));
    }
}
