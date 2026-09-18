namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 5, Title = "Rotting Oranges", Level = Medium, Problem = """
    In the grid, `0` is empty, `1` is a fresh orange and `2` is a rotten orange. Every minute, each rotten orange rots its fresh neighbors
    (up, down, left, right). How many minutes until no fresh orange is left? Return `-1` if some can never rot.
    """)]
public static class RottingOranges
{
    [Approach(Name = "Simulate Minute by Minute", Time = "O((rows · columns)²)", Space = "O(rows · columns)", Idea = """
        Each minute, scan the whole grid and collect the fresh oranges next to a rotten one, then rot them all at once.
        Stop when a minute changes nothing. Every minute rescans the whole grid.
        """)]
    public static int MinutesBySimulation(int[][] grid)
    {
        int minutes = 0;
        while (true)
        {
            var toRot = new List<(int Row, int Column)>();
            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[0].Length; c++)
                {
                    if (grid[r][c] == 1 && HasRottenNeighbor(grid, r, c))
                    {
                        toRot.Add((r, c));
                    }
                }
            }
            if (toRot.Count == 0)
            {
                break;
            }
            foreach (var (r, c) in toRot)
            {
                grid[r][c] = 2;
            }
            minutes++;
        }

        bool anyFresh = grid.Any(row => row.Contains(1));
        return anyFresh ? -1 : minutes;
    }

    private static bool HasRottenNeighbor(int[][] grid, int r, int c)
    {
        return (r > 0 && grid[r - 1][c] == 2)
            || (r < grid.Length - 1 && grid[r + 1][c] == 2)
            || (c > 0 && grid[r][c - 1] == 2)
            || (c < grid[0].Length - 1 && grid[r][c + 1] == 2);
    }

    [Approach(Name = "Multi-Source BFS", Time = "O(rows · columns)", Space = "O(rows · columns)", Idea = """
        Start a BFS from **all** rotten oranges at the same time: put them all in the queue first.
        Each BFS level is one minute. Every orange is visited once, when it rots, so there's no rescanning.
        Count the fresh oranges at the start; if any are left at the end, return `-1`.
        """)]
    public static int MinutesWithBfs(int[][] grid)
    {
        var queue = new Queue<(int Row, int Column)>();
        int fresh = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r][c] == 2)
                {
                    queue.Enqueue((r, c));
                }
                else if (grid[r][c] == 1)
                {
                    fresh++;
                }
            }
        }

        int[] rowSteps = { 1, -1, 0, 0 };
        int[] columnSteps = { 0, 0, 1, -1 };
        int minutes = 0;

        while (queue.Count > 0 && fresh > 0)
        {
            minutes++;
            int rottenThisMinute = queue.Count;
            for (int i = 0; i < rottenThisMinute; i++)
            {
                var (r, c) = queue.Dequeue();
                for (int direction = 0; direction < 4; direction++)
                {
                    int nr = r + rowSteps[direction];
                    int nc = c + columnSteps[direction];
                    if (nr >= 0 && nc >= 0 && nr < grid.Length && nc < grid[0].Length && grid[nr][nc] == 1)
                    {
                        grid[nr][nc] = 2;
                        fresh--;
                        queue.Enqueue((nr, nc));
                    }
                }
            }
        }
        return fresh == 0 ? minutes : -1;
    }

    public static Example[] Examples =>
    [
        new([new[] { new[] { 2, 1, 1 }, new[] { 1, 1, 0 }, new[] { 0, 1, 1 } }], 4),
        new([new[] { new[] { 2, 1, 1 }, new[] { 0, 1, 1 }, new[] { 1, 0, 1 } }], -1),
        new([new[] { new[] { 0, 2 } }], 0),
    ];
}
