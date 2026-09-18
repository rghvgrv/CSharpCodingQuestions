namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 4, Title = "Number of Islands", Level = Medium, Problem = """
    In a map of `'1'` (land) and `'0'` (water), count the islands. An island is land connected up, down, left or right.
    """)]
public static class NumberOfIslands
{
    [Approach(Name = "Flood Fill With DFS", Time = "O(rows · columns)", Space = "O(rows · columns)", Idea = """
        A grid is a graph: each cell connects to its 4 neighbors.

        Scan every cell. When you find land that hasn't been visited, that's a **new island**:
        count it, then "sink" the whole island by turning all of its connected land into water with DFS,
        so it's never counted again.
        """)]
    public static int CountIslands(string[] map)
    {
        char[][] grid = map.Select(row => row.ToCharArray()).ToArray();
        int islands = 0;
        for (int row = 0; row < grid.Length; row++)
        {
            for (int column = 0; column < grid[0].Length; column++)
            {
                if (grid[row][column] == '1')
                {
                    islands++;
                    Sink(grid, row, column);
                }
            }
        }
        return islands;
    }

    private static void Sink(char[][] grid, int row, int column)
    {
        if (row < 0 || column < 0 || row >= grid.Length || column >= grid[0].Length || grid[row][column] != '1')
        {
            return;
        }
        grid[row][column] = '0';
        Sink(grid, row + 1, column);
        Sink(grid, row - 1, column);
        Sink(grid, row, column + 1);
        Sink(grid, row, column - 1);
    }

    public static Example[] Examples =>
    [
        new([new[] { "11110", "11010", "11000", "00000" }], 1),
        new([new[] { "11000", "11000", "00100", "00011" }], 3),
    ];
}
