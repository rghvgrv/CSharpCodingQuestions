namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_04, "Rotting Oranges (Multi-Source BFS)", Medium,
"Grid cells: 0 empty, 1 fresh orange, 2 rotten. Each minute, rotten oranges rot their 4 neighbors. Return minutes until none are fresh, or -1 if impossible.")]
public static class RottingOranges
{
    // Start BFS from ALL rotten oranges at once. Each BFS level = one minute.
    public static int Solve(int[][] grid)
    {
        var queue = new Queue<(int R, int C)>();
        int fresh = 0;
        for (int r = 0; r < grid.Length; r++)
            for (int c = 0; c < grid[0].Length; c++)
                if (grid[r][c] == 2) queue.Enqueue((r, c));
                else if (grid[r][c] == 1) fresh++;

        int minutes = 0;
        (int, int)[] dirs = [(1, 0), (-1, 0), (0, 1), (0, -1)];
        while (queue.Count > 0 && fresh > 0)
        {
            minutes++;
            for (int n = queue.Count; n > 0; n--) // process one level
            {
                var (r, c) = queue.Dequeue();
                foreach (var (dr, dc) in dirs)
                {
                    int nr = r + dr, nc = c + dc;
                    if (nr < 0 || nc < 0 || nr >= grid.Length || nc >= grid[0].Length || grid[nr][nc] != 1) continue;
                    grid[nr][nc] = 2;
                    fresh--;
                    queue.Enqueue((nr, nc));
                }
            }
        }
        return fresh == 0 ? minutes : -1;
    }

    public static void Run()
    {
        Check("[[2,1,1],[1,1,0],[0,1,1]]", Solve([[2, 1, 1], [1, 1, 0], [0, 1, 1]]), 4);
        Check("[[2,1,1],[0,1,1],[1,0,1]]", Solve([[2, 1, 1], [0, 1, 1], [1, 0, 1]]), -1);
        Check("[[0,2]]", Solve([[0, 2]]), 0);
    }
}
