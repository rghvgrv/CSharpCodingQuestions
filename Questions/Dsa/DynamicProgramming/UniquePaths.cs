namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 8, Title = "Unique Paths in a Grid", Level = Medium, Problem = """
    A robot starts in the top-left cell of a `rows × columns` grid and can only move **right** or **down**.
    How many different paths lead to the bottom-right cell? `rows = 3`, `columns = 7` → `28`.
    """)]
public static class UniquePaths
{
    [Approach(Name = "Plain Recursion", Time = "O(2^(rows + columns))", Space = "O(rows + columns)", Idea = """
        You reach a cell either from the cell **above** it or from the cell to its **left**, so
        `Paths(r, c) = Paths(r - 1, c) + Paths(r, c - 1)`. Cells in the first row or column have exactly 1 path.
        """)]
    public static int CountPathsRecursive(int rows, int columns)
    {
        if (rows == 1 || columns == 1)
        {
            return 1;
        }
        return CountPathsRecursive(rows - 1, columns) + CountPathsRecursive(rows, columns - 1);
    }

    [Approach(Name = "Grid Table", Time = "O(rows · columns)", Space = "O(rows · columns)", Idea = """
        Fill a table with the same rule, starting from the top-left. Every cell is computed once.
        """)]
    public static int CountPathsTable(int rows, int columns)
    {
        int[,] paths = new int[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (r == 0 || c == 0)
                {
                    paths[r, c] = 1;
                }
                else
                {
                    paths[r, c] = paths[r - 1, c] + paths[r, c - 1];
                }
            }
        }
        return paths[rows - 1, columns - 1];
    }

    [Approach(Name = "One Row", Time = "O(rows · columns)", Space = "O(columns)", Idea = """
        Each row only needs the row above. Reuse one array: before the update, `row[c]` still holds the value from **above**,
        and `row[c - 1]` has just been updated, so it holds the value from the **left**.
        """)]
    public static int CountPathsOneRow(int rows, int columns)
    {
        int[] row = new int[columns];
        Array.Fill(row, 1);
        for (int r = 1; r < rows; r++)
        {
            for (int c = 1; c < columns; c++)
            {
                row[c] += row[c - 1];
            }
        }
        return row[columns - 1];
    }

    public static Example[] Examples =>
    [
        new([3, 7], 28),
        new([3, 2], 3),
        new([1, 5], 1),
    ];
}
