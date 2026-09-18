namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 7, Title = "N-Queens", Level = Hard, Problem = """
    Place `n` queens on an `n × n` chessboard so that no two attack each other: not in the same row, column or diagonal.
    Return how many different solutions exist. (`n = 8` → `92`.)
    """)]
public static class NQueens
{
    [Approach(Name = "Place Row by Row, Check the Board", Time = "O(n! · n)", Space = "O(n)", Idea = """
        Put one queen in each row. For the current row, try every column. Before placing, check the queens in earlier rows:
        same column, or same diagonal (the row distance equals the column distance)?
        Recurse to the next row, then remove the queen and try the next column (backtracking).
        """)]
    public static int CountByChecking(int n)
    {
        return PlaceChecking(new int[n], 0, n);
    }

    private static int PlaceChecking(int[] queenColumn, int row, int n)
    {
        if (row == n)
        {
            return 1;
        }

        int solutions = 0;
        for (int column = 0; column < n; column++)
        {
            bool safe = true;
            for (int earlier = 0; earlier < row; earlier++)
            {
                int columnDistance = Math.Abs(queenColumn[earlier] - column);
                if (columnDistance == 0 || columnDistance == row - earlier)
                {
                    safe = false;
                    break;
                }
            }
            if (safe)
            {
                queenColumn[row] = column;
                solutions += PlaceChecking(queenColumn, row + 1, n);
            }
        }
        return solutions;
    }

    [Approach(Name = "Remember Used Columns and Diagonals", Time = "O(n!)", Space = "O(n)", Idea = """
        Make the safety check instant with three arrays of flags:

        - `columnUsed[c]`
        - `diagonalUsed[row - column + n]`: every cell on one "↘" diagonal has the same `row - column`
        - `antiDiagonalUsed[row + column]`: every cell on one "↙" diagonal has the same `row + column`

        Set the flags when placing a queen and clear them when backtracking.
        """)]
    public static int CountWithFlags(int n)
    {
        return PlaceWithFlags(0, n, new bool[n], new bool[2 * n], new bool[2 * n]);
    }

    private static int PlaceWithFlags(int row, int n, bool[] columnUsed, bool[] diagonalUsed, bool[] antiDiagonalUsed)
    {
        if (row == n)
        {
            return 1;
        }

        int solutions = 0;
        for (int column = 0; column < n; column++)
        {
            int diagonal = row - column + n;
            int antiDiagonal = row + column;
            if (columnUsed[column] || diagonalUsed[diagonal] || antiDiagonalUsed[antiDiagonal])
            {
                continue;
            }

            columnUsed[column] = diagonalUsed[diagonal] = antiDiagonalUsed[antiDiagonal] = true;
            solutions += PlaceWithFlags(row + 1, n, columnUsed, diagonalUsed, antiDiagonalUsed);
            columnUsed[column] = diagonalUsed[diagonal] = antiDiagonalUsed[antiDiagonal] = false;
        }
        return solutions;
    }

    public static Example[] Examples =>
    [
        new([4], 2),
        new([8], 92),
        new([1], 1),
    ];
}
